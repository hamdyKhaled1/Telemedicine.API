using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Telemedicine.API.Common;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Reports
{
    public class GetAppointmentReportHandler
         : IRequestHandler<GetAppointmentReportQuery, Result<byte[]>>
    {
        private readonly TelemedicineDbContext _context;

        public GetAppointmentReportHandler(TelemedicineDbContext context)
            => _context = context;

        public async Task<Result<byte[]>> Handle(
            GetAppointmentReportQuery request,
            CancellationToken cancellationToken)
        {
            // 1. جيب البيانات
            var data = request.Id.HasValue
                ? await _context.Appointments
                    .Where(a => a.Id == request.Id && !a.IsDeleted)
                    .ToListAsync(cancellationToken)
                : await _context.Appointments
                    .Where(a => !a.IsDeleted)
                    .ToListAsync(cancellationToken);

            // 2. لو Id اتبعت وملقاش حاجة
            if (request.Id.HasValue && !data.Any())
                return Result<byte[]>.Failure(
                    $"Appointment with Id {request.Id} not found or has been deleted.");

            // 3. لو مفيش appointments خالص
            if (!data.Any())
                return Result<byte[]>.Failure("No appointments found.");

            // 4. Generate PDF
            QuestPDF.Settings.License = LicenseType.Community;

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    // Header
                    page.Header().Container().PaddingBottom(10).BorderBottom(1).BorderColor(Colors.Grey.Medium)
                        .Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("Telemedicine System")
                                   .FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                                col.Item().Text("Appointments Report")
                                   .FontSize(13).FontColor(Colors.Grey.Darken2);
                                col.Item().Text($"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC")
                                   .FontSize(9).FontColor(Colors.Grey.Medium);
                            });

                            row.ConstantItem(80).AlignRight().Column(col =>
                            {
                                col.Item().Text($"Total: {data.Count}")
                                   .FontSize(11).Bold().FontColor(Colors.Blue.Darken2);
                            });
                        });

                    // Content
                    page.Content().PaddingTop(15).Column(col =>
                    {
                        // Table Header
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);  // Id
                                columns.RelativeColumn(2);   // Doctor
                                columns.RelativeColumn(2);   // Patient
                                columns.RelativeColumn(2);   // Start
                                columns.RelativeColumn(2);   // End
                                columns.RelativeColumn(1);   // Status
                            });

                            // Table Header Row
                            table.Header(header =>
                            {
                                void HeaderCell(string text) =>
                                    header.Cell().Background(Colors.Blue.Darken2)
                                          .Padding(6).AlignCenter()
                                          .Text(text).FontColor(Colors.White).Bold().FontSize(10);

                                HeaderCell("ID");
                                HeaderCell("Doctor ID");
                                HeaderCell("Patient ID");
                                HeaderCell("Start Time");
                                HeaderCell("End Time");
                                HeaderCell("Status");
                            });

                            // Table Rows
                            foreach (var (item, index) in data.Select((x, i) => (x, i)))
                            {
                                var bgColor = index % 2 == 0 ? Colors.White : Colors.Grey.Lighten3;

                                void DataCell(string text, bool center = true) =>
                                    table.Cell().Background(bgColor).Padding(5)
                                         .Element(center ? x => x.AlignCenter() : x => x.AlignLeft())
                                         .Text(text).FontSize(10);

                                var statusColor = item.Status switch
                                {
                                    AppointmentStatus.Scheduled => Colors.Blue.Medium,
                                    AppointmentStatus.Completed => Colors.Green.Medium,
                                    AppointmentStatus.Canceled => Colors.Red.Medium,
                                    _ => Colors.Grey.Medium
                                };

                                DataCell(item.Id.ToString());
                                DataCell(item.DoctorId.ToString());
                                DataCell(item.PatientId.ToString());
                                DataCell(item.StartTime.ToString("yyyy-MM-dd HH:mm"));
                                DataCell(item.EndTime.ToString("yyyy-MM-dd HH:mm"));

                                // Status بلون مختلف
                                table.Cell().Background(bgColor).Padding(5).AlignCenter()
                                     .Text(item.Status.ToString())
                                     .FontSize(10).FontColor(statusColor).Bold();
                            }
                        });
                    });

                    // Footer
                    page.Footer().AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Page ").FontSize(9).FontColor(Colors.Grey.Medium);
                            text.CurrentPageNumber().FontSize(9).FontColor(Colors.Grey.Medium);
                            text.Span(" of ").FontSize(9).FontColor(Colors.Grey.Medium);
                            text.TotalPages().FontSize(9).FontColor(Colors.Grey.Medium);
                        });
                });
            }).GeneratePdf();

            return Result<byte[]>.Ok(pdf, "Report generated successfully.");
        }
    }
}
