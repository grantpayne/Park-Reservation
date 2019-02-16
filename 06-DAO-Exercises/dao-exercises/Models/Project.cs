using System;
using System.Collections.Generic;
using System.Text;

namespace dao_exercises.Models
{
    class Project
    {
        /// <summary>
        /// The project's id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// The project's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The project's start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The project's end date.
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
