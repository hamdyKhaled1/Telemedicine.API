using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure; 
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Reports
{
    public class GetAppointmentReportHandler
      : IRequestHandler<GetAppointmentReportQuery, byte[]>
    {
        private readonly TelemedicineDbContext _context;

        public GetAppointmentReportHandler(TelemedicineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// If Id provided → returns single appointment
        /// If null → returns all appointments
        /// </summary>
        public async Task<byte[]> Handle(
            GetAppointmentReportQuery request,
            CancellationToken cancellationToken)
        {
            var data = request.Id.HasValue
                ? await _context.Appointments
                    .Where(a => a.Id == request.Id && !a.IsDeleted)
                    .ToListAsync()
                : await _context.Appointments
                    .Where(a => !a.IsDeleted)
                    .ToListAsync();

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Content().Column(col =>
                    {
                        foreach (var item in data)
                        {
                            col.Item().Text($"Appointment #{item.Id}");
                            col.Item().Text(item.StartTime.ToString());
                            col.Item().LineHorizontal(1);
                        }
                    });
                });
            }).GeneratePdf();
        }
    }
}
