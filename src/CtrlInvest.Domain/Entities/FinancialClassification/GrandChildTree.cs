﻿using CtrlInvest.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlInvest.Domain.Entities.FinancialClassification
{
    public class GrandChildTree : EntityBase, INode<ChildTree, ParentTree>
    {
        public GrandChildTree()
            : base()
        {
            this.Children = new HashSet<ParentTree>();
            this.FinancialTransactions = new HashSet<FinancialTransaction>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }

        public int LevelTree { get; set; }
        public ChildTree ParentNode { get; set; }
        public Guid? ParentNodeID { get; set; }
        public ICollection<ParentTree> Children { get; set; }
        public ICollection<FinancialTransaction> FinancialTransactions { get; set; }


        public void Add(ParentTree node)
        {
            throw new NotImplementedException();
        }

        public void Remove(ParentTree node)
        {
            throw new NotImplementedException();
        }
    }
}
