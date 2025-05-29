namespace ApiAcademiaUnifor.ApiService.Dto
{
    public class ClassDto
    {
        public int Id { get; set; }
        public string ClassDuration { get; set; } = string.Empty;
        public string ClassTime { get; set; } = string.Empty;
        public int ClassCapacity { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string ClassDate { get; set; } = string.Empty;
        public string ClassType { get; set; } = string.Empty;
        public int TeacherId {get; set;}
        public List<int>? UserIds { get; set; }
        public bool? ClassCompleted { get; set; }
    }
}
