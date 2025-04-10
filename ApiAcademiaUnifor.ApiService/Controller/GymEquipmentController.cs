﻿using ApiAcademiaUnifor.ApiService.Dto;
using ApiAcademiaUnifor.ApiService.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiAcademiaUnifor.ApiService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class GymEquipmentController(GymEquipmentService _gymEquipmentService) : ControllerBase
    {
        [HttpGet("equipment")]
        public async Task<IActionResult> GetAllEquipments()
        {
            try
            {
                var retorno = await _gymEquipmentService.GetAllEquipments();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("equipment/{id}")]
        public async Task<IActionResult> GetEquipmentById(int id)
        {
            try
            {
                var retorno = await _gymEquipmentService.GetEquipmentById(id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("category")]
        public async Task<IActionResult> GetAllCategorys()
        {
            try
            {
                var retorno = await _gymEquipmentService.GetAllCategorys();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("category/complete")]
        public async Task<IActionResult> GetAllCategoriesWithEquipments()
        {
            try
            {
                var retorno = await _gymEquipmentService.GetAllCategoriesWithEquipments();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("category/complete/{id}")]
        public async Task<IActionResult> GetCategoryCompleteById(int id)
        {
            try
            {
                var retorno = await _gymEquipmentService.GetCategoryCompleteById(id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("equipment")]
        public async Task<IActionResult> PostEquipment(GymEquipmentDto gymEquipmentDto)
        {
            try
            {
                var retorno = await _gymEquipmentService.PostEquipment(gymEquipmentDto);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("category")]
        public async Task<IActionResult> PostCategory(GymEquipmentCategoryInsertDto gymEquipmentCategoryInsertDto)
        {
            try
            {
                var retorno = await _gymEquipmentService.PostCategory(gymEquipmentCategoryInsertDto);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("equipment/{id}")]
        public async Task<IActionResult> PutEquipment(GymEquipmentDto gymEquipmentDto, int id)
        {
            try
            {
                var retorno = await _gymEquipmentService.PutEquipment(gymEquipmentDto, id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("category/{id}")]
        public async Task<IActionResult> PutCategory(GymEquipmentCategoryInsertDto gymEquipmentCategoryInsertDto, int id)
        {
            try
            {
                var retorno = await _gymEquipmentService.PutCategory(gymEquipmentCategoryInsertDto, id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("equipment/{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            try
            {
                var retorno = await _gymEquipmentService.DeleteEquipment(id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("category/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var retorno = await _gymEquipmentService.DeleteCategory(id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
