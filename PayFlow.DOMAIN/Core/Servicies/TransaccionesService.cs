using Microsoft.AspNetCore.Http;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class TransaccionesService : ITransaccionesService
    {
        public readonly ITransaccionesRepository _transaccionesRepository;
        public TransaccionesService(ITransaccionesRepository transaccionesRepository)
        {
            _transaccionesRepository = transaccionesRepository;
        }

        //Get all transacciones
        public async Task<IEnumerable<TransaccionesListDTO>> GetAllTransacciones()
        {
            var transacciones = await _transaccionesRepository.GetAllTransacciones();
            var transaccionesDTO = transacciones.Select(c => new TransaccionesListDTO
            {
                TransaccionId = c.TransaccionId,
                CuentaId = c.CuentaId,
                TipoTransaccion = c.TipoTransaccion,
                Monto = c.Monto,
                FechaHora = c.FechaHora,
                Estado = c.Estado,
                Iporigen = c.Iporigen,
                Ubicacion = c.Ubicacion
            });
            return transaccionesDTO;
        }
        //Get transacciones by id
        public async Task<TransaccionesDTO> GetTransaccionById(int transactionId)
        {
            var transaccion = await _transaccionesRepository.GetTransaccionById(transactionId);
            if (transaccion == null)
            {
                return null;
            }
            var transaccionDTO = new TransaccionesDTO
            {
                TransaccionId = transaccion.TransaccionId,
                CuentaId = transaccion.CuentaId,
                TipoTransaccion = transaccion.TipoTransaccion,
                Monto = transaccion.Monto,
                FechaHora = transaccion.FechaHora,
                NumeroOperacion = transaccion.NumeroOperacion,
                Banco = transaccion.Banco,
                RutaVoucher = transaccion.RutaVoucher,
                ComentariosAdmin = transaccion.ComentariosAdmin,
                Comentario = transaccion.Comentario,
                CuentaDestinoId = transaccion.CuentaDestinoId,
                Iporigen = transaccion.Iporigen,
                Ubicacion = transaccion.Ubicacion
            };
            return transaccionDTO;
        }
        //Add transacciones
        public async Task<int> AddTransaccion(TransaccionesCreateDTO transaccionDTO)
        {
            var transaccion = new Transacciones
            {
                //TransaccionId = transaccionDTO.TransaccionId,
                CuentaId = transaccionDTO.CuentaId,
                TipoTransaccion = transaccionDTO.TipoTransaccion,
                Monto = transaccionDTO.Monto,
                FechaHora = transaccionDTO.FechaHora,
                Estado = transaccionDTO.Estado
            };
            return await _transaccionesRepository.AddTransaccionAsync(transaccion);
        }
        //Update transacciones
        public async Task<bool> UpdateTransaccion(TransaccionesCreateDTO transaccionDTO)
        {
            var transaccion = new Transacciones
            {
                TransaccionId = transaccionDTO.TransaccionId,
                CuentaId = transaccionDTO.CuentaId,
                TipoTransaccion = transaccionDTO.TipoTransaccion,
                Monto = transaccionDTO.Monto,
                FechaHora = transaccionDTO.FechaHora,
                Estado = transaccionDTO.Estado
            };
            return await _transaccionesRepository.UpdateTransaccion(transaccion);
        }
        //Rechazar (Delete) transacciones
        public async Task<bool> RechazarTransaccion(int transaccionId)
        {
            var transaccion = await _transaccionesRepository.GetTransaccionById(transaccionId);
            if (transaccion == null)
            {
                return false;
            }
            transaccion.Estado = "Rechazada";
            return await _transaccionesRepository.UpdateTransaccion(transaccion);
        }
        //Get transacciones by cuentaId
        public async Task<IEnumerable<TransaccionesListDTO>> GetTransaccionesByCuentaId(int cuentaId)
        {
            var transacciones = await _transaccionesRepository.GetTransaccionesByCuentaId(cuentaId);
            var transaccionesDTO = transacciones.Select(c => new TransaccionesListDTO
            {
                TransaccionId = c.TransaccionId,
                CuentaId = c.CuentaId,
                TipoTransaccion = c.TipoTransaccion,
                Monto = c.Monto,
                FechaHora = c.FechaHora,
                Estado = c.Estado,
                Iporigen = c.Iporigen,
                Ubicacion = c.Ubicacion
            });
            return transaccionesDTO;
        }

        // Nuevo método para filtrar por usuario, estado y fechas
        public async Task<IEnumerable<TransaccionesListDTO>> GetTransaccionesByUsuario(int usuarioId, string? estado = null, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var transacciones = await _transaccionesRepository.GetTransaccionesByUsuario(usuarioId, estado, fechaInicio, fechaFin);
            var transaccionesDTO = transacciones.Select(c => new TransaccionesListDTO
            {
                TransaccionId = c.TransaccionId,
                CuentaId = c.CuentaId,
                TipoTransaccion = c.TipoTransaccion,
                Monto = c.Monto,
                FechaHora = c.FechaHora,
                Estado = c.Estado,
                Iporigen = c.Iporigen,
                Ubicacion = c.Ubicacion
            });
            return transaccionesDTO;

        }

        public async Task<ResumenInicioDTO?> ObtenerResumenInicioAsync(int usuarioId)
        {
            return await _transaccionesRepository.ObtenerResumenInicioAsync(usuarioId);
        }
    }
}
