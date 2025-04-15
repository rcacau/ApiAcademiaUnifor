using ApiAcademiaUnifor.ApiService.Dto;
using ApiAcademiaUnifor.ApiService.Models;
using ApiAcademiaUnifor.ApiService.Service.Base;

namespace ApiAcademiaUnifor.ApiService.Service
{
    public class UserService : ServiceBase
    {

        private readonly WorkoutService _workoutService;
        public UserService(Supabase.Client supabase) : base(supabase)
        {
            _workoutService = new WorkoutService(supabase);
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

        public async Task<List<UserCompletoDto>> GetAllCompleteUsers()
        {
            try
            {
                var usuarios = await _supabase.From<Models.User>().Get();

                var tasks = usuarios.Models.Select(async user =>
                {
                    var workoutDtos = await _workoutService.GetWorkoutsByUserId(user.Id);

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
                });

                var userDtos = await Task.WhenAll(tasks);

                return userDtos.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar usuários com treinos: {ex.Message}");
            }
        }

        public async Task<UserCompletoDto> GetCompleteUserByUserId(int id)
        {
            try
            {
                var user = await _supabase.From<Models.User>().Where(x => x.Id == id).Get();
                var userResponse = user.Models.FirstOrDefault();

                if (userResponse == null)
                {
                    throw new Exception("Usuário não encontrado.");
                }

                var workoutDtos = await _workoutService.GetWorkoutsByUserId(userResponse.Id);

                var userCompletoDto = new UserCompletoDto
                {
                    Id = userResponse.Id,
                    Name = userResponse.Name,
                    Email = userResponse.Email,
                    Password = userResponse.Password,
                    Phone = userResponse.Phone,
                    Address = userResponse.Address,
                    BirthDate = userResponse.BirthDate,
                    AvatarUrl = userResponse.AvatarUrl,
                    IsAdmin = userResponse.IsAdmin == true ? true : null,
                    Workouts = workoutDtos
                };

                return userCompletoDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar usuários com treinos: {ex.Message}");
            }
        }

        public async Task<UserDto> Post(UserDto userDto)
        {
            try
            {
                userDto.Id = 0; 

                var lista = await _supabase.From<Models.User>().Get();
                int nextId = lista.Models.Any()
                    ? lista.Models.Max(e => e.Id) + 1
                    : 1; ;

                var user = new Models.User
                {
                    Id = nextId,
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

        public async Task<UserCompletoDto?> Delete(int id)
        {
            try
            {
                var userResponse = await _supabase.From<Models.User>().Where(x => x.Id == id).Single();

                if (userResponse == null)
                    throw new Exception("Usuário não encontrado.");

                var workoutResponse = await _workoutService.GetWorkoutsByUserId(userResponse.Id);

                await _supabase.From<Models.User>().Where(x => x.Id == id).Delete();

                foreach (var workout in workoutResponse)
                {
                    await _workoutService.Delete(workout.Id);
                }

                return new UserCompletoDto
                {
                    Id = userResponse.Id,
                    Name = userResponse.Name,
                    Email = userResponse.Email,
                    Phone = userResponse.Phone,
                    Password = userResponse.Password,
                    Address = userResponse.Address,
                    BirthDate = userResponse.BirthDate,
                    AvatarUrl = userResponse.AvatarUrl,
                    IsAdmin = userResponse.IsAdmin == true ? true : null,
                    Workouts = workoutResponse
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
