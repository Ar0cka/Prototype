using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    private T[] items; // Список нодов
    private int currentItemCount; // текущий размер списка

    public Heap(int maxHeapSize) // экземпляр класса Heap
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item) // Добавление нового нода в список
    {
        item.HeapIndex = currentItemCount; // изменение индекс аййтема на текущий
        items[currentItemCount] = item; // изменения в списке текущего айтема на добавляемый
        SortUp(item); // сортировка вверх
        currentItemCount++; 
    }

    public T RemoveFirst() // удаление первого айтема
    {
        T firstItem = items[0]; // записываем first для того, чтоб добавить в closeList
        currentItemCount--; 
        items[0] = items[currentItemCount]; //первый item = текущему
        items[0].HeapIndex = 0; // hea[Index = 0, что говорит о том, что он первый
        SortDown(items[0]); //Сортировка в низ
        return firstItem; // вернуть певый, чтобы добавить в closeList
    }

    public int Count // возвразает текущий размер списка
    {
        get
        {
            return currentItemCount;
        }
    }

    public void UpdateItem(T item) // Обноваление item
    {
        int compare = item.CompareTo(items[(item.HeapIndex - 1) / 2]);
        if (compare > 0) SortUp(item);
        else SortDown(item);
    }
    
    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item); // равнение объектов на равенство
    }
    
    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1; //индекс что левее
            int childIndexRight = item.HeapIndex * 2 + 2; // индекс что правее
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount) // если левый индекс меньше чем текущий размер
            {
                swapIndex = childIndexLeft; //приравниваем свап индекс к левому индексу

                if (childIndexRight < currentItemCount) //если правый индекс 
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) // строк < 0 
                    {
                        swapIndex = childIndexRight; 
                    }
                }

                if (item.CompareTo(items[swapIndex]) > 0) // строк > 0
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    } // сортировка вних
    
    void SortUp(T item) // сортировка ввех
    {
        int parentIndex = (item.HeapIndex - 1) / 2; //присвоение индекс = Hea[index - 1 / 2
        while (true)
        {
            T parentItem = items[parentIndex]; // присвоение item с индексом parentIndex 
            if (item.CompareTo(parentItem) > 0) //если строк > 0
            {
                Swap(item, parentItem); // меняем местами
            }
            else
            {
                break; 
            }
            parentIndex = (item.HeapIndex - 1) / 2; // создание нового индекса
        }
    }

    void Swap(T itemA, T itemB) // переставляет items местами
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}
