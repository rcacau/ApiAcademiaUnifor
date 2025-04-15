using ApiAcademiaUnifor.ApiService.Dto;
using ApiAcademiaUnifor.ApiService.Models;
using ApiAcademiaUnifor.ApiService.Service.Base;
using Supabase.Gotrue;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Supabase.Gotrue.Mfa;

namespace ApiAcademiaUnifor.ApiService.Service
{
    public class ExerciseService : ServiceBase
    {
        public ExerciseService(Supabase.Client supabase) : base(supabase)
        {
        }

        public async Task<List<ExerciseDto>> GetAll()
        {
            try
            {
                var exercisesResponse = await _supabase.From<Models.Exercise>().Get();
                var exercises = exercisesResponse.Models.Select(e => new ExerciseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Reps = e.Reps,
                    Notes = e.Notes,
                    WorkoutId = e.WorkoutId
                }).ToList();
                return exercises;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ExerciseDto> GetById(int id)
        {
            try
            {
                var result = await _supabase
                .From<Exercise>()
                .Where(e => e.Id == id)
                .Get();

                var exerciseResult = result.Models.FirstOrDefault();

                if (exerciseResult == null)
                    throw new Exception("Equipamento não encontrado");

                return new ExerciseDto
                {
                    Id = exerciseResult.Id,
                    Name = exerciseResult.Name,
                    Reps = exerciseResult.Reps,
                    Notes = exerciseResult.Notes,
                    WorkoutId = exerciseResult.WorkoutId
                };


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ExerciseDto>> GetByWorkoutId(int workoutId)
        {
            try
            {
                var result = await _supabase
                .From<Exercise>()
                .Where(e => e.WorkoutId == workoutId)
                .Get();

                var exerciseResult = result.Models.ToList();

                if (exerciseResult == null)
                    throw new Exception("Equipamento não encontrado");

                return exerciseResult.Select(e => new ExerciseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Reps = e.Reps,
                    Notes = e.Notes,
                    WorkoutId = e.WorkoutId
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ExerciseDto> Post(ExerciseDto exerciseDto)
        {
            try
            {
                exerciseDto.Id = 0;

                var lista = await _supabase.From<Models.Exercise>().Get();

                int nextId = lista.Models.Any() 
                    ? lista.Models.Max(e => e.Id) + 1
                    : 1;

                var exercise = new Models.Exercise
                {
                    Id = nextId,
                    Name = exerciseDto.Name,
                    Reps = exerciseDto.Reps,
                    Notes = exerciseDto.Notes,
                    WorkoutId = exerciseDto.WorkoutId
                };

                var response = await _supabase.From<Models.Exercise>().Insert(exercise);

                return new ExerciseDto
                {
                    Id = response.Models.First().Id,
                    Name = response.Models.First().Name,
                    Reps = response.Models.First().Reps,
                    Notes = response.Models.First().Notes,
                    WorkoutId = response.Models.First().WorkoutId
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ExerciseDto> Put(int id, ExerciseDto exerciseDto)
        {
            try
            {

                var response = await _supabase.From<Models.Exercise>().Where(x => x.Id == id).Single();

                if (response == null)
                    throw new Exception("Usuário não encontrado.");

                response.Name = exerciseDto.Name;
                response.Reps = exerciseDto.Reps;
                response.Notes = exerciseDto.Notes;
                response.WorkoutId = exerciseDto.WorkoutId;

                var exercise = await response.Update<Exercise>();

                var updatedExercise = exercise.Models.FirstOrDefault();

                if (updatedExercise == null)
                    throw new Exception("Erro ao atualizar usuário.");

                return new ExerciseDto
                {
                    Id = updatedExercise.Id,
                    Name = updatedExercise.Name,
                    Reps = updatedExercise.Reps,
                    Notes = updatedExercise.Notes,
                    WorkoutId = updatedExercise.WorkoutId,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ExerciseDto> Delete(int id)
        {
            try
            {
                var response = await _supabase.From<Models.Exercise>().Where(x => x.Id == id).Single();

                if (response == null)
                    throw new Exception("Usuário não encontrado.");

                await _supabase.From<Models.Exercise>().Where(x => x.Id == id).Delete();

                return new ExerciseDto
                {
                    Id = response.Id,
                    Name = response.Name,
                    Reps = response.Reps,
                    Notes = response.Notes,
                    WorkoutId = response.WorkoutId,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}