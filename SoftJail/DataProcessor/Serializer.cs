namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Linq;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {

            var result = context.Prisoners
                .Where(x => ids.Contains(x.Id))
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.FullName,
                    CellNumber = x.Cell.CellNumber,
                    Officers = x.PrisonerOfficers.Select(o => new
                    {
                        OfficerName = o.Officer.FullName,
                        Department = o.Officer.Department.Name
                    })
                    .OrderBy(x => x.OfficerName)
                    .ToList(),
                    TotalOfficerSalary = x.PrisonerOfficers
                        .Sum(x => x.Officer.Salary)
                        .ToString("F2")
                })
                .OrderBy(x => x.TotalOfficerSalary)
                .ThenBy(x => x.Id)
                .ToList();

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);

            return json;

        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            throw new NotImplementedException();
        }
    }
}