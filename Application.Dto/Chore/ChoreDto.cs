namespace Application.Dto.Chore;

public class ChoreDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int IsCompleted { get; set; } = 0;
    public int Remind { get; set; }
    public string Repeat { get; set; }
    public string Priority { get; set; }
    public int Color { get; set; }
}