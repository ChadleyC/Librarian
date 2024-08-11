using Librarian.UI.Models;
using Librarian.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Librarian.UI.Controllers;

public class BookController(IBookService bookService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await bookService.GetAllBooksAsync();
        return View(model);
    }

    [HttpGet]
    public IActionResult Add() => View(new BookModel());

    [HttpPost]
    public async Task<IActionResult> Add(BookModel model)
    {
        var (isValid, resultView) = ProcessModelState(model);
        if (!isValid)
        {
            return resultView;
        }

        try
        {
            var result = await bookService.AddBookAsync(model);

            ViewBag.OperationErrorMessage = (result is null ? "Error adding book, please try again" : null) ?? string.Empty;
            
            return result == null ? View(model) : RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            ViewBag.OperationErrorMessage = e.Message;
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        _ = await bookService.DeleteBookAsync(id);

        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var model = await bookService.GetBookAsync(id);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(BookModel model)
    {
        var (valid, resultView) = ProcessModelState(model);
        if (!valid)
        {
           return resultView;
        }
        
        var (_, err) = await SafelyProcess(() => bookService.EditBookAsync(model));

        if (err == null)
        {
            return RedirectToAction("Index");
        }
        
        ViewBag.OperationErrorMessage = err.Message;
        return View(model);
    }

    private async Task<(T? result, Exception? exception)> SafelyProcess<T>(Func<Task<T>> process)
    {
        try
        {
            return (await process.Invoke(), null);
        }
        catch (Exception e)
        {
            return (default, e);
        }
    }

    private (bool valid, IActionResult view) ProcessModelState<T>(T model)
    {
        if (ModelState.IsValid)
        {
            return (true, Ok());
        }
        
        ViewBag.Errors = ModelState;
        return (false, View(model));

    }
}