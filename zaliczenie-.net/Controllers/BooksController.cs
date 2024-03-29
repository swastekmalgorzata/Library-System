﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zaliczenie_.net.Migrations;
using zaliczenie_.net.Models;

namespace zaliczenie_.net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _dbContext;
        public BooksController(LibraryContext dbContext)
        {
            _dbContext = dbContext;
        }
        //GET:api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Library>>> GetAllBooks()
        {
            if (_dbContext.Books == null)
            {
                return NotFound();
            }
            return await _dbContext.Books.ToListAsync();
        }
        //GET:api/books/5
        [HttpGet("{id}")]

        public async Task<ActionResult<Library>> GetBookById(int id)
        {
            if (_dbContext.Books == null)
            {
                return NotFound();
            }
            var book = await _dbContext.Books.FirstOrDefaultAsync(m => m.idBook == id);
            if (book == null)
            {
                return NotFound();
            }
            return book;
        }

        //POST:api/books
        //[HttpPost]

        //public async Task<ActionResult<Library>> PostBook(Library library)
        //{
        //_dbContext.Books.Add(library);
        // await _dbContext.SaveChangesAsync();
        // return CreatedAtAction(nameof(GetBookById), new { id = library.idBook }, library);
        //}
        [HttpPost]
        public async Task<IActionResult> Create([Bind("idBook,Tittle,Author,Section,Status")] Library library)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(library);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBookById), new { id = library.idBook }, library);
            }
            return (IActionResult)library;
        }
        //DELETE: api/books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (_dbContext.Books == null)
            {
                return NotFound();
            }
            var book = await _dbContext.Books.FirstOrDefaultAsync(m => m.idBook == id);
            if (book == null)
            {
                return NotFound();
            }
            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();

            return Ok("Książka została pomyślnie usunięta.");
        }
        // PUT: api/books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [Bind("Tittle,Author,Section,Status")] Library updatedBook)
        {
            if (id != updatedBook.idBook)
            {
                return BadRequest("Invalid book ID");
            }

            var existingBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.idBook == id);
            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.Tittle = updatedBook.Tittle;
            existingBook.Author = updatedBook.Author;
            existingBook.Section = updatedBook.Section;
            existingBook.Status = updatedBook.Status;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Książka została pomyślnie zaktualizowana.");
        }

        private bool BookExists(int id)
        {
            return _dbContext.Books.Any(b => b.idBook == id);
        }


    }
}
