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

        public IList<Scripture> Scripture { get; set; } 

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SelectList? Notes { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ScriptureNote { get; set; }

        public async Task OnGetAsync(string SortOrder)
        {
            IQueryable<string> notesQuery = from s in _context.Scripture
                                            orderby s.Notes
                                            select s.Notes;

            var scriptures = from s in _context.Scripture
                             select s;

            //sort by date for default
            scriptures = scriptures.OrderBy(s => s.EntryDate);

            
            if (!string.IsNullOrEmpty(SearchString))
            {
                scriptures = scriptures.Where(s => s.Book.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(ScriptureNote))
            {
                scriptures = scriptures.Where(x => x.Notes == ScriptureNote);
            }
           
            Notes = new SelectList(await notesQuery.Distinct().ToListAsync());
            Scripture = await scriptures.ToListAsync();

        }
    }
}
