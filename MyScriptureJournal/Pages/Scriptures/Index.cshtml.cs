using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyScriptureJournal.Data;
using MyScriptureJournal.Models;

namespace MyScriptureJournal.Pages.Scriptures
{
    public class IndexModel : PageModel
    {
        private readonly MyScriptureJournal.Data.MyScriptureJournalContext _context;

        public IndexModel(MyScriptureJournal.Data.MyScriptureJournalContext context)
        {
            _context = context;
        }

        public string BookSort { get; set; }
        public string DateSort { get; set; }

        public IList<Scripture> Scripture { get; set; } 

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public string CurrentFilter { get; set; }

        public SelectList? Notes { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ScriptureNote { get; set; }

        public async Task OnGetAsync(string SortOrder, string SearchString, string ScriptureNote)
        {
            DateSort = String.IsNullOrEmpty(SortOrder) ? "date_sort" : "";
            BookSort = SortOrder == "Book" ? "book_sort" : "Book";

            CurrentFilter = SearchString;

            IQueryable<string> notesQuery = from s in _context.Scripture
                                            orderby s.Notes
                                            select s.Notes;

            var scriptures = from s in _context.Scripture
                             select s;
            
            if (!String.IsNullOrEmpty(SearchString))
            {
                scriptures = scriptures.Where(s => s.Book.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(ScriptureNote))
            {
                scriptures = scriptures.Where(x => x.Notes == ScriptureNote);
            }

            //sort by date for default
            scriptures = SortOrder switch
            {
                "date_sort" => scriptures.OrderBy(s => s.EntryDate),
                "Book" => scriptures.OrderBy(s => s.Book),
                "book_sort" => scriptures.OrderBy(s => s.Book),
                _ => scriptures.OrderBy(s => s.EntryDate),
            };

            Notes = new SelectList(await notesQuery.Distinct().ToListAsync());
            Scripture = await scriptures.AsNoTracking().ToListAsync();

        }
    }
}
