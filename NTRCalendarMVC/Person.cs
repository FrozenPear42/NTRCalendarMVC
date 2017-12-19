namespace NTRCalendarMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Person")]
    public partial class Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Person()
        {
            Attendances = new HashSet<Attendance>();
        }

        public Guid PersonID { get; set; }

        [Required]
        [StringLength(16)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(16)]
        public string LastName { get; set; }

        [Required]
        [StringLength(10)]
        public string UserID { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] timestamp { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
