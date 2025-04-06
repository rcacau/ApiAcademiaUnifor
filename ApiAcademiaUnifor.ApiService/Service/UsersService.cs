﻿using ApiAcademiaUnifor.ApiService.Dto;
using ApiAcademiaUnifor.ApiService.Models;
using Supabase;
using Microsoft.AspNetCore.Mvc;
using Supabase.Gotrue;
using Supabase.Interfaces;

namespace ApiAcademiaUnifor.ApiService.Service
{
    public class UsersService
    {
        private readonly Supabase.Client _supabase;

        public UsersService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<bool> authenticate(AuthenticateDto authenticateDto)
        {
            var user  = await _supabase.From<Users>()
                .Where(x => x.Email == authenticateDto.email && x.Password == authenticateDto.passWord)
                .Single();

            if (user is not null)
                return true;

            return false;
        }

        public async Task<List<UserDto>> GetAll()
        {
            try
            {
                var usuariosResponse = await _supabase.From<Users>().Get();

                var usuarios = usuariosResponse.Models.Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    Address = u.Address,
                    BirthDate = u.BirthDate,
                    AvatarUrl = u.AvatarUrl,
                    IsAdmin = u.IsAdmin,
                    Password = u.Password
                }).ToList();


                return usuarios;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDto> Post(UserInsertDto userInsertDto)
        {
            try
            {
                var lista = await _supabase.From<Users>().Get();

                int id = 0;

                foreach (var item in lista.Models)
                {
                    if (item.Id > id)
                    {
                        id = item.Id;
                    }
                }

                var user = new Users
                {
                    Id = id + 1,
                    Password = userInsertDto.Password,
                    Name = userInsertDto.Name,
                    Email = userInsertDto.Email,
                    Phone = userInsertDto.Phone,
                    Address = userInsertDto.Address,
                    BirthDate = userInsertDto.BirthDate,
                    AvatarUrl = userInsertDto.AvatarUrl,
                    IsAdmin = userInsertDto.IsAdmin
                };

                var usuariosResponse = await _supabase.From<Users>().Insert(user);



                var usuario = new UserDto{
                    Id = usuariosResponse.Models.FirstOrDefault().Id,
                    Name = usuariosResponse.Models.FirstOrDefault().Name,
                    Email = usuariosResponse.Models.FirstOrDefault().Email,
                    Phone = usuariosResponse.Models.FirstOrDefault().Phone,
                    Address = usuariosResponse.Models.FirstOrDefault().Address,
                    BirthDate = usuariosResponse.Models.FirstOrDefault().BirthDate,
                    AvatarUrl = usuariosResponse.Models.FirstOrDefault().AvatarUrl,
                    IsAdmin = usuariosResponse.Models.FirstOrDefault().IsAdmin,
                    Password = usuariosResponse.Models.FirstOrDefault().Password
                };


                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDto> Put(UserInsertDto userInsertDto, int id)
        {
            try
            {
                
                var userResponse = await _supabase.From<Users>().Where(x => x.Id == id).Single();

                if (userResponse == null)
                    throw new Exception("Usuário não encontrado.");

                userResponse.Name = userInsertDto.Name;
                userResponse.Email = userInsertDto.Email;
                userResponse.Password = userInsertDto.Password;
                userResponse.Phone = userInsertDto.Phone;
                userResponse.Address = userInsertDto.Address;
                userResponse.BirthDate = userInsertDto.BirthDate;
                userResponse.AvatarUrl = userInsertDto.AvatarUrl;
                userResponse.IsAdmin = userInsertDto.IsAdmin;


                var user = await userResponse.Update<Users>();
                
                var usuario = new UserDto
                {
                    Id = user.Models.FirstOrDefault().Id,
                    Name = user.Models.FirstOrDefault().Name,
                    Email = user.Models.FirstOrDefault().Email,
                    Phone = user.Models.FirstOrDefault().Phone,
                    Password = user.Models.FirstOrDefault().Password,
                    Address = user.Models.FirstOrDefault().Address,
                    BirthDate = user.Models.FirstOrDefault().BirthDate,
                    AvatarUrl = user.Models.FirstOrDefault().AvatarUrl,
                    IsAdmin = user.Models.FirstOrDefault().IsAdmin
                };

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDto?> Delete(int id)
        {
            try
            {
                var userResponse = await _supabase.From<Users>().Where(x => x.Id == id).Single();

                if (userResponse == null)
                    throw new Exception("Usuário não encontrado.");

                await _supabase.From<Users>().Where(x => x.Id == id).Delete();

                var usuario = new UserDto
                {
                    Id = userResponse.Id,
                    Name = userResponse.Name,
                    Email = userResponse.Email,
                    Phone = userResponse.Phone,
                    Password = userResponse.Password,
                    Address = userResponse.Address,
                    BirthDate = userResponse.BirthDate,
                    AvatarUrl = userResponse.AvatarUrl,
                    IsAdmin = userResponse.IsAdmin
                };

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}
