using Microsoft.AspNetCore.Http;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class RetiroService : IRetiroService
    {
        public readonly ITransaccionesRepository _transaccionesRepository;
        // private readonly ICuentasRepository _cuentasRepository;
        public RetiroService(ITransaccionesRepository transaccionesRepository)
        {
            _transaccionesRepository = transaccionesRepository;
            // _cuentasRepository = cuentasRepository;
        }

        
        //Get draw transacciones by id
        public async Task<RetiroDTO> GetRetiroById(int transactionId)
        {
            var transaccion = await _transaccionesRepository.GetTransaccionById(transactionId);
            if (transaccion == null)
            {
                return null;
            }
            var retiroDTO = new RetiroDTO
            {
                TransaccionId = transaccion.TransaccionId,
                CuentaId = transaccion.CuentaId,
                Monto = transaccion.Monto,
                Iporigen = transaccion.Iporigen
            };
            return retiroDTO;
        }

        //Add Draw transacciones
        public async Task<int> AddRetiro(RetiroCreateDTO retiroCreateDTO, string Iporigen)
        {
            // Validate the transaccion object before adding
            if (retiroCreateDTO.CuentaId <= 0 || retiroCreateDTO.Monto <= 0)
            {
                return -1; // Indicate an error with invalid data
            }

            /*
            var cuenta = await _cuentasRepository.GetCuentaById(retiroCreateDTO.CuentaId);
            if (cuenta == null)
                throw new InvalidOperationException("Cuenta no encontrada.");

            // Validar saldo suficiente
            if (cuenta.Saldo < retiroCreateDTO.Monto)
                throw new InvalidOperationException("Saldo insuficiente para realizar el retiro.");
            */

            var transaccion = new Transacciones
            {
                CuentaId = retiroCreateDTO.CuentaId,
                TipoTransaccion = "Retiro",
                Monto = retiroCreateDTO.Monto,
                FechaHora = DateTime.Now,
                Estado = "Aceptado",
                Iporigen = Iporigen
            };

            return await _transaccionesRepository.AddTransaccion(transaccion);
        }
    }
}
