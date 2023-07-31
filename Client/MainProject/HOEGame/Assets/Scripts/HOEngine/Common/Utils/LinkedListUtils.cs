using System;
using System.Collections.Generic;

namespace HOEngine.Resources
{
    public static class LinkedListUtils
    {
        public static void Sort<T>(this LinkedList<T> lnk, Func<T, T, int> compare)
        {
            LinkedListNode<T> cNode;
            LinkedListNode<T> pNode;
            LinkedListNode<T> tNode;
            cNode = lnk.First;
            int result;
            bool IsSwitch;
            while (cNode != lnk.Last)
            {
                tNode = cNode;
                pNode = cNode;
                IsSwitch = false;
                do
                {
                    pNode = pNode.Next;
                    result = compare(tNode.Value, pNode.Value);
                    if (result > 0)
                    {
                        tNode = pNode;
                        IsSwitch = true;
                    }
                } while (pNode != lnk.Last);
                if (IsSwitch)
                {
                    lnk.Remove(tNode);
                    lnk.AddBefore(cNode, tNode);
                }
                cNode = tNode.Next;
            }
        }
    }
}