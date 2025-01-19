namespace Szakdoga.Models
{
    public interface IDevice
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public IEntity[] Entities { get; set; }
    }
}
