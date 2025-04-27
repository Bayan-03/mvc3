using EventMVC3.Models;
using EventMVC3.ModelView; 

namespace EventMVC3.Helpers 
{
    public static class EventExtensions
    {
        public static void MapFromViewModel(this Event entity, EventModelViewForEdit viewModel)
        {
            entity.Name = viewModel.Name;
            entity.StartDate = viewModel.StartDate;
            entity.FinishDate = viewModel.FinishDate;
            entity.StartTime = viewModel.StartTime;
            entity.FineshTime = viewModel.FineshTime;
            entity.PlaceName = viewModel.PlaceName;
            entity.City = viewModel.City;
            entity.Discription = viewModel.Discription;
            entity.Category = viewModel.Category;
        }
    }
}
