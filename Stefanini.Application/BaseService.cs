using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stefanini.Application
{
    public class BaseService : IBaseService
    {
        private readonly IMapper _mapper;

        public BaseService(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public T Map<T>(object source)
        {
            return this._mapper.Map<T>(source);
        }
    }
}
