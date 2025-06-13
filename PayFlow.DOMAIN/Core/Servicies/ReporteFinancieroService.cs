using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Data;
using ClosedXML.Excel;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class ReporteFinancieroService : IReporteFinancieroService
    {
        private readonly PayflowContext _context;

        public ReporteFinancieroService(PayflowContext context)
        {
            _context = context;
        }
        //Generar reporte financiero basado en filtros
        public async Task<List<ReporteFinancieroItemDTO>> GenerarReporteAsync(ReporteFinancieroFiltroDTO filtro)
        {
            var query = _context.Transacciones
                .Include(t => t.Cuenta)
                    .ThenInclude(c => c.Usuario)
                .AsQueryable();

            if (filtro.FechaInicio.HasValue)
                query = query.Where(t => t.FechaHora >= filtro.FechaInicio.Value);
            if (filtro.FechaFin.HasValue)
                query = query.Where(t => t.FechaHora <= filtro.FechaFin.Value);
            if (!string.IsNullOrEmpty(filtro.TipoTransaccion))
                query = query.Where(t => t.TipoTransaccion == filtro.TipoTransaccion);
            if (!string.IsNullOrEmpty(filtro.Estado))
                query = query.Where(t => t.Estado == filtro.Estado);
            if (filtro.UsuarioId.HasValue)
                query = query.Where(t => t.Cuenta.UsuarioId == filtro.UsuarioId);

            var lista = await query.ToListAsync();

            return lista.Select(t => new ReporteFinancieroItemDTO
            {
                TransaccionId = t.TransaccionId,
                NombreUsuario = t.Cuenta.Usuario.Nombres + " " + t.Cuenta.Usuario.Apellidos,
                TipoTransaccion = t.TipoTransaccion,
                Monto = t.Monto,
                FechaHora = t.FechaHora,
                Estado = t.Estado,
                IpOrigen = t.Iporigen ?? "",
                Ubicacion = ObtenerUbicacionDesdeIP(t.Iporigen),
                Comentarios = t.ComentariosAdmin,
                EsSospechosa = EsTransaccionSospechosa(t)
            }).ToList();
        }
        // Verifica si la transacción es sospechosa según criterios definidos
        private bool EsTransaccionSospechosa(Transacciones t)
        {
            return t.Monto > 10000 ||
                   _context.Transacciones.Count(x => x.CuentaId == t.CuentaId &&
                                                     x.FechaHora >= t.FechaHora.AddMinutes(-2) &&
                                                     x.FechaHora <= t.FechaHora.AddMinutes(2)) > 3;
        }
        // Obtiene la ubicación geográfica a partir de la IP (dummy o implementación real)
        private string ObtenerUbicacionDesdeIP(string ip)
        {
            // Dummy: retornar "Desconocido" o implementar API externa (GeoIP)
            return "Desconocido";
        }

        public async Task<byte[]> ExportarReporteCSVAsync(List<ReporteFinancieroItemDTO> data)
        {
            using var writer = new StringWriter();
            writer.WriteLine("ID,Usuario,Tipo,Monto,FechaHora,Estado,IP,Ubicación,Comentarios,Sospechosa");
            foreach (var item in data)
            {
                writer.WriteLine($"{item.TransaccionId},{item.NombreUsuario},{item.TipoTransaccion},{item.Monto}," +
                    $"{item.FechaHora},{item.Estado},{item.IpOrigen},{item.Ubicacion},{item.Comentarios},{item.EsSospechosa}");
            }
            return Encoding.UTF8.GetBytes(writer.ToString());
        }
        // Exporta el reporte a formato Excel usando ClosedXML
        public async Task<byte[]> ExportarReporteExcelAsync(List<ReporteFinancieroItemDTO> data)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Reporte");

            // Encabezados
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Usuario";
            worksheet.Cell(1, 3).Value = "Tipo";
            worksheet.Cell(1, 4).Value = "Monto";
            worksheet.Cell(1, 5).Value = "FechaHora";
            worksheet.Cell(1, 6).Value = "Estado";
            worksheet.Cell(1, 7).Value = "IP";
            worksheet.Cell(1, 8).Value = "Ubicación";
            worksheet.Cell(1, 9).Value = "Comentarios";
            worksheet.Cell(1, 10).Value = "Sospechosa";

            // Filas
            for (int i = 0; i < data.Count; i++)
            {
                var row = i + 2;
                var item = data[i];
                worksheet.Cell(row, 1).Value = item.TransaccionId;
                worksheet.Cell(row, 2).Value = item.NombreUsuario;
                worksheet.Cell(row, 3).Value = item.TipoTransaccion;
                worksheet.Cell(row, 4).Value = item.Monto;
                worksheet.Cell(row, 5).Value = item.FechaHora;
                worksheet.Cell(row, 6).Value = item.Estado;
                worksheet.Cell(row, 7).Value = item.IpOrigen;
                worksheet.Cell(row, 8).Value = item.Ubicacion;
                worksheet.Cell(row, 9).Value = item.Comentarios;
                worksheet.Cell(row, 10).Value = item.EsSospechosa ? "Sí" : "No";
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

    }

}
