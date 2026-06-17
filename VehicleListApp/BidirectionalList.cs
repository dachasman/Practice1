using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace VehicleListApp
{
    public class BidirectionalList
    {
        private ListNode head;
        private ListNode tail;
        private int count;
        private ListNode currentIteratorNode;

        public BidirectionalList()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public int Length => count;
        public ListNode Head => head;

        public VehicleRecord this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                    throw new IndexOutOfRangeException("Індекс поза межами списку.");
                
                ListNode current = head;
                for (int i = 0; i < index; i++) current = current.Next;
                return current.Value;
            }
            set
            {
                if (index < 0 || index >= count)
                    throw new IndexOutOfRangeException("Індекс поза межами списку.");

                ListNode current = head;
                for (int i = 0; i < index; i++) current = current.Next;
                current.Value = value;
            }
        }

        public void AddAt(int index, VehicleRecord item)
        {
            if (index < 0 || index > count) throw new ArgumentOutOfRangeException(nameof(index));

            ListNode newNode = new ListNode(item);

            if (count == 0)
            {
                head = newNode;
                tail = newNode;
            }
            else if (index == 0)
            {
                newNode.Next = head;
                head.Prev = newNode;
                head = newNode;
            }
            else if (index == count)
            {
                tail.Next = newNode;
                newNode.Prev = tail;
                tail = newNode;
            }
            else
            {
                ListNode current = head;
                for (int i = 0; i < index; i++) current = current.Next;

                newNode.Next = current;
                newNode.Prev = current.Prev;
                current.Prev.Next = newNode;
                current.Prev = newNode;
            }
            count++;
        }

        public VehicleRecord RemoveFromStart()
        {
            if (count == 0) throw new InvalidOperationException("Список порожній.");

            VehicleRecord removedData = head.Value;
            if (count == 1)
            {
                head = null;
                tail = null;
            }
            else
            {
                head = head.Next;
                head.Prev = null;
            }
            count--;
            return removedData;
        }

        public VehicleRecord GetInitialValue()
        {
            if (head == null) return null;
            currentIteratorNode = head;
            return currentIteratorNode.Value;
        }

        public VehicleRecord GetNextValue()
        {
            if (currentIteratorNode == null || currentIteratorNode.Next == null)
            {
                currentIteratorNode = null;
                return null;
            }
            currentIteratorNode = currentIteratorNode.Next;
            return currentIteratorNode.Value;
        }

        public void Reverse()
        {
            if (count <= 1) return;
            ListNode current = head;
            ListNode temp = null;

            while (current != null)
            {
                temp = current.Prev;
                current.Prev = current.Next;
                current.Next = temp;
                current = current.Prev;
            }
            temp = head;
            head = tail;
            tail = temp;
        }

        // Пошук через предикат (як на скріншоті 6)
        public BidirectionalList Search(Func<VehicleRecord, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            BidirectionalList results = new BidirectionalList();
            ListNode current = head;
            while (current != null)
            {
                if (predicate(current.Value))
                {
                    results.AddAt(results.Length, current.Value);
                }
                current = current.Next;
            }
            return results;
        }

        // Серіалізація в JSON (як на скріншоті 6)
        public void Serialize(string filePath)
        {
            List<VehicleRecord> itemList = new List<VehicleRecord>();
            ListNode current = head;
            while (current != null)
            {
                itemList.Add(current.Value);
                current = current.Next;
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(itemList, options);
            File.WriteAllText(filePath, jsonString);
        }

        // Десеріалізація з JSON
        public void Deserialize(string filePath)
        {
            if (!File.Exists(filePath)) return;

            string jsonString = File.ReadAllText(filePath);
            var itemList = JsonSerializer.Deserialize<List<VehicleRecord>>(jsonString);

            head = null;
            tail = null;
            count = 0;

            if (itemList == null) return;
            foreach (var item in itemList)
            {
                AddAt(count, item);
            }
        }
    }
}