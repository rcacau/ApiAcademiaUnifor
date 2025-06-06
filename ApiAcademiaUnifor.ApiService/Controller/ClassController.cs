﻿using Microsoft.AspNetCore.Mvc;
using ApiAcademiaUnifor.ApiService.Service;
using ApiAcademiaUnifor.ApiService.Dto;

namespace ApiAcademiaUnifor.ApiService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController(ClassService _classService) : ControllerBase
    {
        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeUser([FromBody] SubscribeRequest subscribeRequest)
        {
            try
            {
                var retorno = await _classService.SubscribeUser(subscribeRequest.ClassId, subscribeRequest.UserId);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        
        [HttpPost("unsubscribe")]
        public async Task<IActionResult> UnsubscribeUser([FromBody] SubscribeRequest subscribeRequest)
        {
            try
            {
                var retorno = await _classService.UnsubscribeUser(subscribeRequest.ClassId, subscribeRequest.UserId);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var retorno = await _classService.GetAll();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("incomplete")]
        public async Task<IActionResult> GetIGetIncompleteClasses()
        {
            try
            {
                var retorno = await _classService.GetIncompleteClasses();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var retorno = await _classService.GetById(id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClassDto classDto)
        {
            try
            {
                var retorno = await _classService.Post(classDto);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ClassDto classDto)
        {
            try
            {
                var retorno = await _classService.Put(classDto, id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var retorno = await _classService.Delete(id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
