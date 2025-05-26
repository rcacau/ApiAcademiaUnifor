using ApiAcademiaUnifor.ApiService.Dto;
using ApiAcademiaUnifor.ApiService.Models;
using ApiAcademiaUnifor.ApiService.Service.Base;

namespace ApiAcademiaUnifor.ApiService.Service
{
    public class ClassService : ServiceBase
    {
        public ClassService(Supabase.Client supabase) : base(supabase)
        {
        }

        public async Task<List<ClassDto>> GetAll()
        {
            try
            {
                var classesResponse = await _supabase.From<Classes>().Get();

                var classes = classesResponse.Models.Select(c => new ClassDto
                {
                    Id = c.Id,
                    ClassName = c.ClassName,
                    ClassType = c.ClassType,
                    ClassDate = c.ClassDate,
                    ClassTime = c.ClassTime,
                    ClassDuration = c.ClassDuration,
                    ClassCapacity = c.ClassCapacity,
                    TeacherId = c.TeacherId,
                    UserIds = c.ClassListUsers
                }).ToList();

                return classes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ClassDto> GetById(int id)
        {
            try
            {
                var classResponse = await _supabase.From<Classes>()
                    .Where(x => x.Id == id)
                    .Single();

                if (classResponse == null)
                    throw new Exception("Aula não encontrada.");

                return new ClassDto
                {
                    Id = classResponse.Id,
                    ClassName = classResponse.ClassName,
                    ClassType = classResponse.ClassType,
                    ClassDate = classResponse.ClassDate,
                    ClassTime = classResponse.ClassTime,
                    ClassDuration = classResponse.ClassDuration,
                    ClassCapacity = classResponse.ClassCapacity,
                    TeacherId = classResponse.TeacherId,
                    UserIds = classResponse.ClassListUsers
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ClassDto> Post(ClassDto classDto)
        {
            try
            {
                classDto.Id = 0;

                var lista = await _supabase.From<Classes>().Get();
                int nextId = lista.Models.Any()
                    ? lista.Models.Max(e => e.Id) + 1
                    : 1;

                var newClass = new Classes
                {
                    Id = nextId,
                    ClassName = classDto.ClassName,
                    ClassType = classDto.ClassType,
                    ClassDate = classDto.ClassDate,
                    ClassTime = classDto.ClassTime,
                    ClassDuration = classDto.ClassDuration,
                    ClassCapacity = classDto.ClassCapacity,
                    TeacherId = classDto.TeacherId,
                    ClassListUsers = classDto.UserIds ?? new List<int>()
                };

                var classResponse = await _supabase.From<Classes>().Insert(newClass);

                var insertedClass = classResponse.Models.FirstOrDefault();

                if (insertedClass == null)
                    throw new Exception("Erro ao inserir aula.");

                return new ClassDto
                {
                    Id = insertedClass.Id,
                    ClassName = insertedClass.ClassName,
                    ClassType = insertedClass.ClassType,
                    ClassDate = insertedClass.ClassDate,
                    ClassTime = insertedClass.ClassTime,
                    ClassDuration = insertedClass.ClassDuration,
                    ClassCapacity = insertedClass.ClassCapacity,
                    TeacherId = insertedClass.TeacherId,
                    UserIds = insertedClass.ClassListUsers
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ClassDto> Put(ClassDto classDto, int id)
        {
            try
            {
                var classResponse = await _supabase.From<Classes>().Where(x => x.Id == id).Single();

                if (classResponse == null)
                    throw new Exception("Aula não encontrada.");

                classResponse.ClassName = classDto.ClassName;
                classResponse.ClassType = classDto.ClassType;
                classResponse.ClassDate = classDto.ClassDate;
                classResponse.ClassTime = classDto.ClassTime;
                classResponse.ClassDuration = classDto.ClassDuration;
                classResponse.ClassCapacity = classDto.ClassCapacity;
                classResponse.TeacherId = classDto.TeacherId;
                classResponse.ClassListUsers = classDto.UserIds ?? new List<int>();

                var updatedClass = await classResponse.Update<Classes>();

                var result = updatedClass.Models.FirstOrDefault();

                if (result == null)
                    throw new Exception("Erro ao atualizar aula.");

                return new ClassDto
                {
                    Id = result.Id,
                    ClassName = result.ClassName,
                    ClassType = result.ClassType,
                    ClassDate = result.ClassDate,
                    ClassTime = result.ClassTime,
                    ClassDuration = result.ClassDuration,
                    ClassCapacity = result.ClassCapacity,
                    TeacherId = result.TeacherId,
                    UserIds = result.ClassListUsers // Retorna os IDs atualizados
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ClassDto?> Delete(int id)
        {
            try
            {
                var classResponse = await _supabase.From<Classes>().Where(x => x.Id == id).Single();

                if (classResponse == null)
                    throw new Exception("Aula não encontrada.");

                await _supabase.From<Classes>().Where(x => x.Id == id).Delete();

                return new ClassDto
                {
                    Id = classResponse.Id,
                    ClassName = classResponse.ClassName,
                    ClassType = classResponse.ClassType,
                    ClassDate = classResponse.ClassDate,
                    ClassTime = classResponse.ClassTime,
                    ClassDuration = classResponse.ClassDuration,
                    ClassCapacity = classResponse.ClassCapacity,
                    TeacherId = classResponse.TeacherId,
                    UserIds = classResponse.ClassListUsers // Retorna os IDs dos usuários
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}