using AutoMapper;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Mappers
{
   public class RateMapper
    {
        IMapper mapper;
        public RateMapper()
        {
            mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Rate, RateDataModel>().ReverseMap();
            }).CreateMapper();
        }
        public List<RateDataModel> Map(IEnumerable<Rate> rates)
        {
            List<RateDataModel> listOfRateDataModels = new List<RateDataModel>();
            foreach (var rate in rates)
            {
                listOfRateDataModels.Add(mapper.Map<RateDataModel>(rate));
            }

            return listOfRateDataModels;
        }
            
        
       
    }
}
