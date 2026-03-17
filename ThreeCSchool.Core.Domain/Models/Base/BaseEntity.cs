# nullable disable
namespace ThreeCSchool.Core.Domain.Models.Base
{
    public abstract class BaseEntity<TKey> 
    {
        public TKey Id { get; set; } 
        
    }
}
