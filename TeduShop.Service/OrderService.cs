using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduShop.Data.Infrastructure;
using TeduShop.Data.Repositories;
using TeduShop.Model.Models;

namespace TeduShop.Service
{
    public interface IOrderService
    {
        Order Create(Order order,List<OrderDetail> orderDetails);
    }
    public class OrderService : IOrderService
    {
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository;
        IUnitOfWork _unitOfWork;

        //cẩn thận và tinh tế trong từng câu từng chũ n

        public OrderService(IOrderRepository orderRepository,IOrderDetailRepository orderDetailRepository,IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._unitOfWork = unitOfWork;
        }

        //Hàm thêm vào order và orderDetail 
        public Order Create(Order order, List<OrderDetail> orderDetails) 
        {
            try
            {
                _orderRepository.Add(order);
                _unitOfWork.Commit();//luu

                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderID = order.ID;
                    _orderDetailRepository.Add(orderDetail);
                }

                _unitOfWork.Commit();//luu
                return order;
            }
            catch
            {
                throw;
            }
            
        }
    }
}
