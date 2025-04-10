using ApiAcademiaUnifor.ApiService.Dto;
using ApiAcademiaUnifor.ApiService.Models;
using ApiAcademiaUnifor.ApiService.Service.Base;

namespace ApiAcademiaUnifor.ApiService.Service
{
    public class UserService : ServiceBase
    {
        public UserService(Supabase.Client supabase) : base(supabase)
        {
        }

        public async Task<bool> authenticate(AuthenticateDto authenticateDto)
        {
            var user = await _supabase.From<Models.User>()
                .Where(x => x.Email == authenticateDto.email && x.Password == authenticateDto.passWord)
                .Single();

            return user is not null;
        }

        public async Task<List<UserDto>> GetAll()
        {
            try
            {
                var usuariosResponse = await _supabase.From<Models.User>().Get();

                var usuarios = usuariosResponse.Models.Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    Address = u.Address,
                    BirthDate = u.BirthDate,
                    AvatarUrl = u.AvatarUrl,
                    IsAdmin = u.IsAdmin == true ? true : null,
                    Password = u.Password
                }).ToList();

                return usuarios;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDto> Post(UserDto userDto)
        {
            try
            {
                userDto.Id = 0; 

                var lista = await _supabase.From<Models.User>().Get();
                int id = lista.Models.Any() ? lista.Models.Max(e => e.Id) : 0;

                var user = new Models.User
                {
                    Id = id + 1,
                    Password = userDto.Password,
                    Name = userDto.Name,
                    Email = userDto.Email,
                    Phone = userDto.Phone,
                    Address = userDto.Address,
                    BirthDate = userDto.BirthDate,
                    AvatarUrl = userDto.AvatarUrl,
                    IsAdmin = userDto.IsAdmin == true ? true : null
                };

                var usuariosResponse = await _supabase.From<Models.User>().Insert(user);

                var insertedUser = usuariosResponse.Models.FirstOrDefault();

                if (insertedUser == null)
                    throw new Exception("Erro ao inserir usuário.");

                return new UserDto
                {
                    Id = insertedUser.Id,
                    Name = insertedUser.Name,
                    Email = insertedUser.Email,
                    Phone = insertedUser.Phone,
                    Address = insertedUser.Address,
                    BirthDate = insertedUser.BirthDate,
                    AvatarUrl = insertedUser.AvatarUrl,
                    IsAdmin = insertedUser.IsAdmin == true ? true : null,
                    Password = insertedUser.Password
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDto> Put(UserDto userDto, int id)
        {
            try
            {
                var userResponse = await _supabase.From<Models.User>().Where(x => x.Id == id).Single();

                if (userResponse == null)
                    throw new Exception("Usuário não encontrado.");

                userResponse.Name = userDto.Name;
                userResponse.Email = userDto.Email;
                userResponse.Password = userDto.Password;
                userResponse.Phone = userDto.Phone;
                userResponse.Address = userDto.Address;
                userResponse.BirthDate = userDto.BirthDate;
                userResponse.AvatarUrl = userDto.AvatarUrl;
                userResponse.IsAdmin = userDto.IsAdmin == true ? true : null;

                var user = await userResponse.Update<Models.User>();

                var updatedUser = user.Models.FirstOrDefault();

                if (updatedUser == null)
                    throw new Exception("Erro ao atualizar usuário.");

                return new UserDto
                {
                    Id = updatedUser.Id,
                    Name = updatedUser.Name,
                    Email = updatedUser.Email,
                    Phone = updatedUser.Phone,
                    Password = updatedUser.Password,
                    Address = updatedUser.Address,
                    BirthDate = updatedUser.BirthDate,
                    AvatarUrl = updatedUser.AvatarUrl,
                    IsAdmin = updatedUser.IsAdmin == true ? true : null
                };
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
                var userResponse = await _supabase.From<Models.User>().Where(x => x.Id == id).Single();

                if (userResponse == null)
                    throw new Exception("Usuário não encontrado.");

                await _supabase.From<Models.User>().Where(x => x.Id == id).Delete();

                return new UserDto
                {
                    Id = userResponse.Id,
                    Name = userResponse.Name,
                    Email = userResponse.Email,
                    Phone = userResponse.Phone,
                    Password = userResponse.Password,
                    Address = userResponse.Address,
                    BirthDate = userResponse.BirthDate,
                    AvatarUrl = userResponse.AvatarUrl,
                    IsAdmin = userResponse.IsAdmin == true ? true : null
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<UserCompletoDto>> GetWorkoutExercise()
        {
            try
            {
                var usuarios = await _supabase.From<Models.User>().Get();
                var treinos = await _supabase.From<Workout>().Get();
                var exercicios = await _supabase.From<Exercise>().Get();

                var userDtos = usuarios.Models.Select(user =>
                {
                    var workoutDtos = treinos.Models
                        .Where(w => w.UserId == user.Id)
                        .Select(w =>
                        {
                            var exerciseDtos = exercicios.Models
                                .Where(e => e.WorkoutId == w.Id)
                                .Select(e => new ExerciseDto
                                {
                                    Name = e.Name,
                                    Reps = e.Reps,
                                    Notes = e.Notes
                                })
                                .ToList();

                            return new WorkoutDto
                            {
                                Name = w.Name,
                                Description = w.Description,
                                Exercises = exerciseDtos
                            };
                        })
                        .ToList();

                    return new UserCompletoDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Password = user.Password,
                        Phone = user.Phone,
                        Address = user.Address,
                        BirthDate = user.BirthDate,
                        AvatarUrl = user.AvatarUrl,
                        IsAdmin = user.IsAdmin == true ? true : null,
                        Workouts = workoutDtos
                    };
                }).ToList();

                return userDtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar usuários com treinos: {ex.Message}");
            }
        }
    }
}
