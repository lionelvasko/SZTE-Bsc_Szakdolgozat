namespace SomfyAPI.Models
{
    public class Command
    {
        public int Id { get; set; }
        public required string CommandName { get; set; }
        public required int Nparams { get; set; }
    }
}
