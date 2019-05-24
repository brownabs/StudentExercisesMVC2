using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC2.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        [Required]
        [StringLength(55)]
        public string Title { get; set; }

        [Required]
        public string ExerciseLanguage { get; set; }
    }
}
