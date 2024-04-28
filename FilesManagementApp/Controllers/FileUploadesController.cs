using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Files.Domain.Models;
using Infrastructer;
using File.Infrastructure.Repositories;
using Files.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FilesManagementApp.Controllers
{
    [Authorize]
    public class FileUploadesController : Controller
    {
        private readonly IRepository<FileUploade> context;

        public FileUploadesController(IRepository<FileUploade> context)
        {
            this.context = context;
        }

        // GET: FileUploades
        public async Task<IActionResult> Index()
        {
            //عرض جميع الملفات المتوفرة في الصفحة الرئيسية 
            return View(context.GetAll());
        }

        // GET: FileUploades/Details/5
        //عرض معلومات الملف في حال وجوده 
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileUploade = context.Get(id);
            //في حال عدم العثور على الملف 
            if (fileUploade == null)
            {
                return NotFound();
            }
            //عرض معلومات الملف في حال وجوده
            return View(fileUploade);
        }

        // GET: FileUploades/Create

        public IActionResult Create()
        {
            return View();
        }

        // POST: FileUploades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        //FileUploadForCreation يحوي بيانات تعريفية حول الملف
        public async Task<IActionResult> Create(IFormFile formFile, [Bind("Name,Description,CreatedBy")] FileUploadForCreation fileUploade)
        {

            if (ModelState.IsValid)
            {

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", formFile.FileName);

                //إنشاء ملف جديد في المسار المحدد
                using (var stream = System.IO.File.Create(filepath))
                {
                    //نسخ محتوى الملف المُحمل إلى تدفق الملف stream
                    await formFile.CopyToAsync(stream);
                }

                //Maper
                var file = new FileUploade();

                file.Name = fileUploade.Name;
                file.Description = fileUploade.Description;
                file.CreatedBy = fileUploade.CreatedBy;
                file.FileType = formFile.ContentType;
                file.Path = filepath;

                context.Add(file);

                context.SavaChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(fileUploade);
        }
             // GET: FileUploades/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileUploade = context.Get(id);
            if (fileUploade == null)
            {
                return NotFound();
            }
            return View(fileUploade);
        }

        // POST: FileUploades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult Edit(Guid id, [Bind("Id,Name,Path,FileType,Description,CreatedOn,CreatedBy")] FileUploade fileUploade)
        {
            if (id != fileUploade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(fileUploade);
                    context.SavaChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileUploadeExists(fileUploade.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(fileUploade);
        }

        // GET: FileUploades/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileUploade = context.Get(id);
            if (fileUploade == null)
            {
                return NotFound();
            }

            return View(fileUploade);
        }

        // POST: FileUploades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public IActionResult DeleteConfirmed(Guid id)
        {
            var fileUploade = context.Get(id);
            if (fileUploade != null)
            {
                context.Delete(fileUploade);
            }

            context.SavaChanges();
            return RedirectToAction(nameof(Index));
        }
        //دالة تساعدنا في التحقق من وجود كائن  
        private bool FileUploadeExists(Guid id)
        {
            var obje = context.Get(id);
            if (obje == null)
                return false;
            return true;
        }


        //لإدارة تنزيل الملفات
        
        public IActionResult Download(Guid id)
        {
            var file = context.Get(id);

            if (file == null)
                return NotFound();

            //استخراج اسم الملف من المسار 
            var filename = System.IO.Path.GetFileName(file.Path);
            //بناء المسار الكامل للملف على الخادم
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", filename);
            //التحقق من وجود الملف على الخادم 
            if (!System.IO.File.Exists(path))
                return NotFound();
            //تخزين محتوى الملف 
            var memory = new MemoryStream();
            //فتح قراءة الملف على الخادم
            using (var stream = new FileStream(path, FileMode.Open))
            {
                // نسخ محتوى الملف من الخادم إلى MemoryStream
                stream.CopyTo(memory);
            }
            //إعادة ضبط موقع قراءة الذاكرة الى البداية
            memory.Position = 0;
            // إرجاع الملف للمتصفح للتنزيل
            return File(memory, file.FileType, Path.GetFileName(path));
        }

    }
}
