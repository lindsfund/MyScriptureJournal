using System;
using System.ComponentModel.DataAnnotations;

namespace MyScriptureJournal.Models
{
	public class Scripture
	{
		public int Id { get; set; }

		[DataType(DataType.Date)]
		public DateTime EntryDate { get; set; }

		public string Book { get; set; }
		public int Chapter { get; set; }
		public string? Verse { get; set; }
        public string Text { get; set; }
		public string? Notes { get; set; }
	}
}

