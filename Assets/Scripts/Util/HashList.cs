using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Misner.Utility.Collections
{
	/// <summary>
	/// Hash list. Uses the strengths of a LinkedList and a Dictionary together at the cost of hosting a copy of each.
	/// Note: This only means two copies of the items if they are value types.
	/// 
	/// Pros:
	/// -Fast access. Has the look up speed of a dictionary, and the enumuration speed of a list. Making all forms of access faster for large values of Count.
	/// 
	/// Cons:
	/// -Double memory. Since this is a wrapper for two types of collection, you get the memory overhead of each. So don't use this on your watch?
	/// -Double writes. You're doing double the writes, even if the collections are covering eachothers weaknesses.
	/// </summary>
	public class HashList<T> : ICollection<T> {
		
		#region Wrapped Collections
		
		/// <summary>
		/// A collection of items in a LinkedList. Unlike a List<T>, we won't have to reindex our Dictionary when values are added / removed from within the list.
		/// </summary>
		private LinkedList<T> itemList = new LinkedList<T>();
		
		/// <summary>
		/// A dictionary mapping of item values to their node placement in the above list.
		/// </summary>
		private Dictionary<T, LinkedListNode<T>> itemToListNode = new Dictionary<T, LinkedListNode<T>>();
		
		#endregion
		
		#region ICollection implementation
		
		/// <summary>
		/// Add the specified item. Amortized O(1).
		/// </summary>
		/// <param name="item">Item.</param>
		public void Add (T item) {
            if (!itemToListNode.ContainsKey(item)) {
				LinkedListNode<T> node = itemList.AddLast(item);
				
				itemToListNode[item] = node;
            }
		}
		
		/// <summary>
		/// Clear this instance. O(N).
		/// </summary>
		public void Clear () {
			itemList.Clear();
			itemToListNode.Clear();
		}
		
		/// <summary>
		/// Contains the specified item. Amortized O(1), not O(N) like a list.
		/// </summary>
		/// <param name="item">Item.</param>
		public bool Contains (T item) {
			return itemToListNode.ContainsKey(item);
		}
		
		/// <summary>
		/// Copies to array. O(N), not O(N log(N)) list a dictionary.
		/// </summary>
		/// <param name="array">Array.</param>
		/// <param name="arrayIndex">Array index.</param>
		public void CopyTo (T[] array, int arrayIndex) {
			itemList.CopyTo(array, arrayIndex);
		}
		
		/// <summary>
		/// Remove the specified item. Amortized O(1), not O(N) like a list.
		/// </summary>
		/// <param name="item">Item.</param>
		public bool Remove (T item) {
			if (itemToListNode.ContainsKey(item)) {
				LinkedListNode<T> node = itemToListNode[item];
				
				itemList.Remove(node);
				itemToListNode.Remove(item);
				
				return true;
			} else {
				return false;
			}
		}
		
		/// <summary>
		/// Gets the count. O(1).
		/// </summary>
		/// <value>The count.</value>
		public int Count {
			get {
				return itemList.Count;
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether this instance is read only. O(1).
		/// </summary>
		/// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
		public bool IsReadOnly {
			get {
				return false;
			}
		}
		
		#endregion
		
		#region IEnumerable implementation
		
		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<T> GetEnumerator () {
			return itemList.GetEnumerator();
		}
		
		#endregion
		
		#region IEnumerable implementation
		
		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator () {
			return itemList.GetEnumerator();
		}
		
		#endregion
		
	}
}
