namespace NTRCalendarMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Appointment")]
    public partial class Appointment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Appointment()
        {
            Attendances = new HashSet<Attendance>();
        }

        public Guid AppointmentID { get; set; }

        [Required(ErrorMessage = "Tytu³ jest wymagany")]
        [StringLength(16)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Opis jest wymagany")]
        [StringLength(50)]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Start jest wymagany")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Koniec jest wymagany")]
        public TimeSpan EndTime { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        [Timestamp]
        public byte[] timestamp { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }

        public override string ToString()
        {
            return $"{AppointmentDate} {StartTime} - {EndTime}: {Title}";
        }
    }
}
