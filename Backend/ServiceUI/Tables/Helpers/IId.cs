using System.ComponentModel.DataAnnotations;

namespace ServiceUI.Tables.Helpers
{
    public abstract class IId
    {
        [Key]
        public Guid Id { get; set; }
    }
}
