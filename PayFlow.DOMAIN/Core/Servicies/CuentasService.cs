using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class CuentasService : ICuentasService
    {
        public readonly ICuentasRepository _cuentasRepository;
        public CuentasService(ICuentasRepository cuentasRepository)
        {
            _cuentasRepository = cuentasRepository;
        }
        //Get cuenta by Id
        public async Task<CuentasDTO?> GetCuentaById(int cuentaId)
        {
            var cuenta = await _cuentasRepository.GetCuentaByIdAsync(cuentaId);
            if (cuenta == null)
            {
                return null;
            }
            var cuentaDTO = new CuentasDTO
            {
                CuentaId = cuenta.CuentaId,
                UsuarioId = cuenta.UsuarioId,
                Saldo = cuenta.Saldo,
                EstadoCuenta = cuenta.EstadoCuenta,
                NumeroCuenta = cuenta.NumeroCuenta
            };
            return cuentaDTO;
        }
        //Get cuenta by usuarioId
        public async Task<CuentaUserDTO?> GetCuentaByUsuarioId(int usuarioId)
        {
            var cuenta= await _cuentasRepository.GetCuentaByUsuarioId(usuarioId);
            if (cuenta == null)
            {
                return null;
            }
            var cuentaUserDTO = new CuentaUserDTO
            {
                CuentaId = cuenta.CuentaId,
                UsuarioId = cuenta.UsuarioId,
                Saldo = cuenta.Saldo,
                NumeroCuenta = cuenta.NumeroCuenta
            };
            return cuentaUserDTO;
        }
        // Update cuenta
        public async Task<bool> UpdateCuenta(CuentasDTO cuentaDTO)
        {
            // Obtener la entidad original desde el repositorio
            var cuenta = await _cuentasRepository.GetCuentaByIdAsync(cuentaDTO.CuentaId);
            if (cuenta == null)
            {
                return false;
            }
            // Actualizar solo las propiedades necesarias
            cuenta.Saldo = cuentaDTO.Saldo;
            cuenta.NumeroCuenta = cuentaDTO.NumeroCuenta;
            cuenta.EstadoCuenta = cuentaDTO.EstadoCuenta;
            // Guardar cambios usando la misma instancia
            await _cuentasRepository.UpdateCuentaAsync(cuenta);
            return true;
        }
    }
}
