using ApiAcademiaUnifor.ApiService.Service.Base;
using ApiAcademiaUnifor.ApiService.Models;
using ApiAcademiaUnifor.ApiService.Dto;

namespace ApiAcademiaUnifor.ApiService.Service
{
    public class WorkoutService : ServiceBase
    {
        private readonly ExerciseService _exerciseService;

        public WorkoutService(Supabase.Client supabase) : base(supabase)
        {
            _exerciseService = new ExerciseService(supabase);
        }

        public async Task<List<WorkoutDto>> GetAll()
        {
            try
            {
                var workoutsResponse = await _supabase.From<Workout>().Get();
                var workouts = workoutsResponse.Models.ToList();

                var workoutDtos = new List<WorkoutDto>();

                foreach (var w in workouts)
                {
                    var exercises = await _exerciseService.GetExercisesByWorkoutId(w.Id);

                    workoutDtos.Add(new WorkoutDto
                    {
                        Id = w.Id,
                        UserId = w.UserId,
                        Name = w.Name,
                        Description = w.Description,
                        Exercises = exercises
                    });
                }

                return workoutDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<WorkoutDto>> GetAllWorkoutsByUserId(int userId)
        {
            try
            {
                var workoutsResponse = await _supabase.From<Workout>().Where(x => x.UserId == userId).Get();
                var workouts = workoutsResponse.Models.ToList();

                var workoutDtos = new List<WorkoutDto>();

                foreach (var w in workouts)
                {
                    var exercises = await _exerciseService.GetExercisesByWorkoutId(w.Id);

                    workoutDtos.Add(new WorkoutDto
                    {
                        Id = w.Id,
                        UserId = w.UserId,
                        Name = w.Name,
                        Description = w.Description,
                        Exercises = exercises
                    });
                }

                return workoutDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<WorkoutDto> GetById(int id)
        {
            try
            {
                var workoutsResponse = await _supabase.From<Workout>().Where(x => x.Id == id).Get();
                var workout = workoutsResponse.Models.FirstOrDefault();

                if (workout == null)
                {
                    throw new Exception($"Workout with ID {id} not found.");
                }

                var exercises = await _exerciseService.GetExercisesByWorkoutId(id);
                var workoutDto = new WorkoutDto
                {
                    Id = workout.Id,
                    UserId = workout.UserId,
                    Name = workout.Name,
                    Description = workout.Description,
                    Exercises = exercises
                };

                return workoutDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<WorkoutDto> Post(WorkoutDto workoutDto)
        {
            try
            {
                var lista = await _supabase.From<Workout>().Get();
                int id = lista.Models.Any() ? lista.Models.Max(e => e.Id) : 0;
                var workout = new Workout
                {
                    Id = id + 1,
                    UserId = workoutDto.UserId,
                    Name = workoutDto.Name,
                    Description = workoutDto.Description
                };
                await _supabase.From<Workout>().Insert(workout);
                return workoutDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<WorkoutDto> Put(int id, WorkoutDto workoutDto)
        {
            try
            {
                var workout = await _supabase.From<Workout>().Where(x => x.Id == id).Get();
                if (workout.Models.Count == 0)
                {
                    throw new Exception($"Workout with ID {id} not found.");
                }
                var updatedWorkout = new Workout
                {
                    Id = id,
                    UserId = workoutDto.UserId,
                    Name = workoutDto.Name,
                    Description = workoutDto.Description
                };
                await _supabase.From<Workout>().Where(x => x.Id == id).Update(updatedWorkout);
                return workoutDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<WorkoutDto> Delete(int id)
        {
            try
            { 
                var workout = await _supabase.From<Workout>().Where(x => x.Id == id).Get();
                var workoutResponse = workout.Models.FirstOrDefault();
                if (workout.Models.Count == 0 || workoutResponse is null)
                {
                    throw new Exception($"Workout with ID {id} not found.");
                }

                await _supabase.From<Workout>().Where(x => x.Id == id).Delete();

                var exercises = await _exerciseService.GetExercisesByWorkoutId(id);

                foreach (var exercise in exercises)
                {
                    await _exerciseService.Delete(exercise.Id);
                }

                var workoutDto = new WorkoutDto 
                {
                    Id = id,
                    UserId = workoutResponse.UserId,
                    Name = workoutResponse.Name,
                    Description = workoutResponse.Description,
                    Exercises = exercises
                };



                return workoutDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}