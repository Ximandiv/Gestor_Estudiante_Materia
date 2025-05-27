namespace Domain.ViewModels;

public class StudentListVM
{
    public List<StudentVM> Students { get; set; } = new List<StudentVM>();
    public int TotalCount { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public StudentListVM(
        List<StudentVM> students,
        int totalCount,
        int pageSize = 10,
        int pageNumber = 1)
    {
        Students = students;
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalCount = totalCount;
    }
}
