﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Services.Events;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nop.Services.Orders
{
    /// <summary>
    /// RewardPoints service interface
    /// </summary>
    public partial class RewardPointsService: IRewardPointsService
    {
        #region Fields

        private readonly IRepository<RewardPointsHistory> _rphRepository;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly IStoreContext _storeContext;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="rphRepository">RewardPointsHistory repository</param>
        /// <param name="rewardPointsSettings">Reward points settings</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="eventPublisher">Event published</param>
        public RewardPointsService(IRepository<RewardPointsHistory> rphRepository,
            RewardPointsSettings rewardPointsSettings,
            IStoreContext storeContext,
            IEventPublisher eventPublisher)
        {
            this._rphRepository = rphRepository;
            this._rewardPointsSettings = rewardPointsSettings;
            this._storeContext = storeContext;
            this._eventPublisher = eventPublisher;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get reward points for customer
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        /// <returns>PointsBalance</returns>

        public int GetRewardPointsBalance(int customerId, int storeId)
        {
            //int result = 0;
            //var lastRph = _rphRepository.Table
            //        .Where(x=>x.CustomerId == customerId)
            //        .OrderByDescending(rph => rph.CreatedOnUtc)
            //        .ThenByDescending(rph => rph.Id)
            //        .FirstOrDefault();
            //if (lastRph != null)
            //    result = lastRph.PointsBalance;
            //return result;

            var query = _rphRepository.Table;
            if (customerId > 0)
                query = query.Where(rph => rph.CustomerId == customerId);
            if (!_rewardPointsSettings.PointsAccumulatedForAllStores)
                query = query.Where(rph => rph.StoreId == storeId);
            query = query.OrderByDescending(rph => rph.CreatedOnUtc).ThenByDescending(rph => rph.Id);

            var lastRph = query.FirstOrDefault();
            return lastRph != null ? lastRph.PointsBalance : 0;

        }

        /// <summary>
        /// Add reward points
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        /// <param name="points">Points</param>
        /// <param name="message">Message</param>
        /// <param name="usedWithOrderId">Used with OrderId</param>
        /// <param name="usedAmount">Used amount</param>
        /// <returns>RewardPointsHistory</returns>

        public RewardPointsHistory AddRewardPointsHistory(int customerId, int points, int storeId,  string message = "",
           int usedWithOrderId = 0, decimal usedAmount = 0M)
        {

            var rewardPointsHistory = new RewardPointsHistory
            {
                CustomerId = customerId,
                UsedWithOrderId = usedWithOrderId,
                Points = points,
                PointsBalance = GetRewardPointsBalance(customerId, storeId) + points,
                UsedAmount = usedAmount,
                Message = message,
                CreatedOnUtc = DateTime.UtcNow
            };
            _rphRepository.Insert(rewardPointsHistory);

            //event notification
            _eventPublisher.EntityInserted(rewardPointsHistory);

            return rewardPointsHistory;
        }

        public IList<RewardPointsHistory> GetRewardPointsHistory(int customerId = 0, bool showHidden = false)
        {
            //var query = _rewardPointsHistory.Table
            //        .Where(x => x.CustomerId == customerId)
            //        .OrderByDescending(rph => rph.CreatedOnUtc)
            //        .ThenByDescending(rph => rph.Id);

            //return query.ToList();
            var query = _rphRepository.Table;
            if (customerId > 0)
                query = query.Where(rph => rph.CustomerId == customerId);
            if (!showHidden && !_rewardPointsSettings.PointsAccumulatedForAllStores)
            {
                //filter by store
                var currentStoreId = _storeContext.CurrentStore.Id;
                query = query.Where(rph => rph.StoreId == currentStoreId);
            }
            query = query.OrderByDescending(rph => rph.CreatedOnUtc).ThenByDescending(rph => rph.Id);

            var records = query.ToList();
            return records;

        }

        #endregion
    }
}
