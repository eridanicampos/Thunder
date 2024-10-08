using AutoMapper;
using ProjectTest.Application.DTO.EnderecoEntrega;
using ProjectTest.Application.DTO.Pedido;
using ProjectTest.Application.DTO.Tarefa;
using ProjectTest.Application.DTO.User;
using ProjectTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.AutoMapper
{
    public class AutoMapperSetup : Profile
    {
        public AutoMapperSetup() 
        {
            #region DTOtoDomain

            CreateMap<UserDTO, Usuario>();
            CreateMap<UserModifyDTO, Usuario>();
            CreateMap<UserParmersDTO, Usuario>();

            CreateMap<EnderecoEntregaDTO, EnderecoEntrega>();
            CreateMap<EnderecoEntregaModifyDTO, EnderecoEntrega>();
            CreateMap<EnderecoEntregaParamDTO, EnderecoEntrega>();

            CreateMap<PedidoDTO, Pedido>();
            CreateMap<PedidoModifyDTO, Pedido>();
            CreateMap<PedidoParamDTO, Pedido>();

            CreateMap<TarefaDTO, Tarefa>();
            CreateMap<TarefaModifyDTO, Tarefa>();
            CreateMap<TarefaParamDTO, Tarefa>();

            #endregion

            #region DomaintoDTO

            CreateMap<Usuario, UserDTO>();
            CreateMap<Usuario, UserModifyDTO>();
            CreateMap<Usuario, UserParmersDTO>();

            CreateMap<EnderecoEntrega, EnderecoEntregaDTO>();
            CreateMap<EnderecoEntrega,  EnderecoEntregaModifyDTO> ();
            CreateMap<EnderecoEntrega, EnderecoEntregaParamDTO>();

            CreateMap<Pedido, PedidoDTO>();
            CreateMap<Pedido, PedidoModifyDTO>();
            CreateMap<Pedido , PedidoParamDTO>();

            CreateMap<Tarefa, TarefaDTO>();
            CreateMap<Tarefa, TarefaModifyDTO>();
            CreateMap<Tarefa, TarefaParamDTO>();
            #endregion

        }
    }
}
