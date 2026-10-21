using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Search.Analysis.Utils
{
    /// <summary>
    /// An information reTRIEval tree, a.k.a., a prefix tree. A Trie is similar to a
    /// dictionary, except that keys must be strings. Furthermore, Trie provides an
    /// efficient means (GetPrefixedBy()) to find all values given just a PREFIX of a
    /// key.
    /// <p>
    /// 
    /// All retrieval operations run in O(nm) time, where n is the size of the
    /// key/prefix and m is the size of the alphabet. Some implementations may reduce
    /// this to O(n log m) or even O(n) time. Insertion operations are assumed to be
    /// infrequent and may be slower. The space required is roughly linear with
    /// respect to the sum of the sizes of all keys in the tree, though this may be
    /// reduced if many keys have common prefixes.
    /// <p>
    /// 
    /// The Trie can be set to ignore case. Doing so is the same as making all keys
    /// and prefixes lower case. That means the original keys cannot be extracted
    /// from the Trie.
    /// <p>
    /// 
    /// Restrictions (not necessarily limitations!)
    /// <ul>
    /// <li><b>This class is not synchronized.</b> Do that externally if you
    /// desire.
    /// <li>Keys and values may not be null.
    /// <li>The interface to this is not complete.
    /// </ul>
    /// 
    /// See http://www.csse.monash.edu.au/~lloyd/tildeAlgDS/Tree/Trie.html for a
    /// discussion of Tries.
    /// </summary>
    public class Trie<S, V>
    {
        /// <summary>
        /// Our representation consists of a tree of nodes whose edges are labelled
        /// by strings. The first characters of all labels of all edges of a node
        /// must be distinct. Typically the edges are sorted, but this is determined
        /// by TrieNode.
        /// <p>
        /// 
        /// An abstract TrieNode<V> is a mapping from String keys to values, { <K1, V1>,
        /// ..., <KN, VN> }, where all Ki and Kj are distinct for all i != j. For any
        /// node N, define KEY(N) to be the concatenation of all labels on the edges
        /// from the root to that node. Then the abstraction function is:
        /// <p>
        /// 
        /// <blockquote> { <KEY(N), N.GetValue() | N is a child of root and
        /// N.GetValue() != null} </blockquote>
        /// 
        /// An earlier version used character labels on edges. This made
        /// implementation simpler but used more memory because one node would be
        /// allocated to each character in long strings if that string had no common
        /// prefixes with other elements of the Trie.
        /// <p>
        /// 
        /// <dl>
        /// <dt>INVARIANT:</td>
        /// <dd>For any node N, for any edges Ei and Ej from N,<br>
        /// i != j &lt;==&gt; Ei.getLabel().getCharAt(0) !=
        /// Ej.getLabel().getCharAt(0)</dd>
        /// <dd>Also, all invariants for TrieNode<V> and TrieEdge must hold.</dd>
        /// </dl>
        /// </summary>
        private TrieNode<V> root;

        /// <summary>
        /// Indicates whever search keys are case-sensitive or not. If true, keys
        /// will be canonicalized to lowercase.
        /// </summary>
        private bool ignoreCase;

        /// <summary>
        /// The constant EmptyIterator to return when nothing matches.
        /// </summary>
        private static readonly IEnumerator EMPTY_ITERATOR = new EmptyIterator();

        /// <summary>
        /// Constructs a new, empty tree.
        /// </summary>
        public Trie(bool ignoreCase)
        {
            this.ignoreCase = ignoreCase;
            Clear();
        }

        /// <summary>
        /// Makes this empty.
        /// 
        /// @modifies this
        /// </summary>
        public virtual void Clear()
        {
            this.root = new TrieNode<V>();
        }

        /// <summary>
        /// Returns the canonical version of the given string.
        /// <p>
        /// 
        /// In the basic version, strings are added and searched without
        /// modification. So this simply returns its parameter s.
        /// <p>
        /// 
        /// Other overrides may also perform a conversion to the NFC form
        /// (interoperable across platforms) or to the NFKC form after removal of
        /// accents and diacritics from the NFKD form (ideal for searches using
        /// strings in natural language).
        /// <p>
        /// 
        /// Made public instead of protected, because the public Prefix operations
        /// below may need to use a coherent conversion of search prefixes.
        /// </summary>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: public String CanonicalCase(final String s)
        public virtual string CanonicalCase(string s)
        {
            if (!ignoreCase)
            {
                return s;
            }

            return s.ToUpper(System.Globalization.CultureInfo.GetCultureInfo("en-US")).ToLower(System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }

        /// <summary>
        /// Matches the pattern <tt>b</tt> against the text
        /// <tt>a[startOffset...stopOffset - 1]</tt>.
        /// </summary>
        /// <returns> the first <tt>j</tt> so that:<br>
        ///         <tt>0 &lt;= i &lt; b.Length</tt> AND<br>
        ///         <tt>a[startOffset + j] != b[j]</tt> [a and b differ]<br>
        ///         OR <tt>stopOffset == startOffset + j</tt> [a is undefined];<br>
        ///         Returns -1 if no such <tt>j</tt> exists, i.e., there is a
        ///         match.<br>
        ///         Examples:
        ///         <ol>
        ///         <li>a = "abcde", startOffset = 0, stopOffset = 5, b = "abc"<br>
        ///         abcde ==&gt; returns -1<br>
        ///         abc
        ///         <li>a = "abcde", startOffset = 1, stopOffset = 5, b = "bXd"<br>
        ///         abcde ==&gt; returns 1 bXd
        ///         <li>a = "abcde", startOffset = 1, stopOffset = 3, b = "bcd"<br>
        ///         abc ==&gt; returns 2<br>
        ///         bcd
        ///         </ol>
        /// 
        /// @requires 0 &lt;= startOffset &lt;= stopOffset &lt;= a.Length </returns>
        private int Match(string a, int startOffset, int stopOffset, string b)
        {
            // j is an index into b
            // i is a parallel index into a
            int i = startOffset;
            for (int j = 0; j < b.Length; j++)
            {
                if (i >= stopOffset)
                {
                    return j;
                }
                if (a[i] != b[j])
                {
                    return j;
                }
                i++;
            }
            return -1;
        }


        /// <summary>
        /// Maps the given key (which may be empty) to the given value.
        /// </summary>
        /// <returns> the old value associated with key, or <tt>null</tt> if none
        /// @requires value != null
        /// @modifies this </returns>
        public virtual V Add(string key, V value)
        {
            // early conversion of key, for best performance
            key = CanonicalCase(key);
            // Find the largest prefix of key, key[0..i - 1], already in this.
            TrieNode<V> node = root;
            int i = 0;
            while (i < key.Length)
            {
                // Find the edge whose label starts with key[i].
                TrieEdge<V> edge = node.Get(key[i]);
                if (edge == null)
                {
                    // 1) Additive insert.
                    TrieNode<V> newNode = new TrieNode<V>(value);
                    node.Put(key.Substring(i), newNode);
                    return default(V);
                }
                // Now check that rest of label matches
                string label = edge.Label;
                int j = Match(key, i, key.Length, label);
                if (j >= 0)
                {
                    // 2) Prefix overlaps perfectly with just part of edge label
                    // Do split insert as follows...
                    //
                    // node node ab = label
                    // ab | ==> a | a = label[0...j - 1] (inclusive)
                    // child intermediate b = label[j...] (inclusive)
                    // b / \ c c = key[i + j...] (inclusive)
                    // child newNode
                    //
                    // ...unless c = "", in which case you just do a "splice
                    // insert" by ommiting newNew and setting intermediate's value.
                    TrieNode<V> child = edge.Child;
                    TrieNode<V> intermediate = new TrieNode<V>();
                    string a = label.Substring(0, j);
                    // Assert.that(CanonicalCase(a).Equals(a), "Bad edge a");
                    string b = label.Substring(j);
                    // Assert.that(CanonicalCase(b).Equals(b), "Bad edge a");
                    string c = key.Substring(i + j);
                    if (c.Length > 0)
                    {
                        // Split.
                        TrieNode<V> newNode = new TrieNode<V>(value);
                        node.Remove(label[0]);
                        node.Put(a, intermediate);
                        intermediate.Put(b, child);
                        intermediate.Put(c, newNode);
                    }
                    else
                    {
                        // Splice.
                        node.Remove(label[0]);
                        node.Put(a, intermediate);
                        intermediate.Put(b, child);
                        intermediate.Value = value;
                    }
                    return default(V);
                }
                // Prefix overlaps perfectly with all of edge label.
                // Keep searching.
                node = edge.Child;
                i += label.Length;
            }
            // 3) Relabel insert. Prefix already in this, though not necessarily
            // associated with a value.
            V ret = node.Value;
            node.Value = value;
            return ret;
        }

        /// <summary>
        /// Returns the node associated with prefix, or null if none. (internal)
        /// </summary>
        private TrieNode<V> Fetch(string prefix)
        {
            // This private method uses prefixes already in canonical form.
            TrieNode<V> node = root;
            for (int i = 0; i < prefix.Length; )
            {
                // Find the edge whose label starts with prefix[i].
                TrieEdge<V> edge = node.Get(prefix[i]);
                if (edge == null)
                {
                    return null;
                }
                // Now check that rest of label matches.
                string label = edge.Label;
                int j = Match(prefix, i, prefix.Length, label);
                if (j != -1)
                {
                    return null;
                }
                i += label.Length;
                node = edge.Child;
            }
            return node;
        }

        /// <summary>
        /// Returns the value associated with the given key, or null if none.
        /// </summary>
        /// <returns> the <tt>Object</tt> value or <tt>null</tt> </returns>
        public virtual object Get(string key)
        {
            // early conversion of search key
            key = CanonicalCase(key);
            // search the node associated with key, if it exists
            TrieNode<V> node = Fetch(key);
            if (node == null)
            {
                return null;
            }
            // key exists, return the value
            return node.Value;
        }

        /// <summary>
        /// Ensures no values are associated with the given key.
        /// </summary>
        /// <returns> <tt>true</tt> if any values were actually removed
        /// @modifies this </returns>
        public virtual bool Remove(string key)
        {
            // early conversion of search key
            key = CanonicalCase(key);
            // search the node associated with key, if it exists
            TrieNode<V> node = Fetch(key);
            if (node == null)
            {
                return false;
            }
            // key exists and can be removed.
            // TODO: prune unneeded nodes to save space
            bool ret = node.Value != null;
            node.Value = default(V);
            return ret;
        }

        /// <summary>
        /// Returns an iterator (of Object) of the values mapped by keys in this that
        /// start with the given prefix, in any order. That is, the returned iterator
        /// contains exactly the values v for which there exists a key k so that
        /// k.StartsWith(prefix) and Get(k) == v. The Remove() operation on the
        /// iterator is unimplemented.
        /// 
        /// @requires this not modified while iterator in use
        /// </summary>
        public virtual IEnumerator GetPrefixedBy(string prefix)
        {
            // Early conversion of search key
            prefix = CanonicalCase(prefix);
            // Note that canonicalization MAY have changed the prefix length!
            return GetPrefixedBy(prefix, 0, prefix.Length);
        }

        /// <summary>
        /// Same as GetPrefixedBy(prefix.Substring(startOffset, stopOffset). This is
        /// useful as an optimization in certain applications to avoid allocations.
        /// <p>
        /// 
        /// Important: canonicalization of prefix substring is NOT performed here!
        /// But it can be performed early on the whole buffer using the public method
        /// <tt>CanonicalCase(String)</tt> of this.
        /// 
        /// @requires 0 &lt;= startOffset &lt;= stopOffset &lt;= prefix.Length </summary>
        /// <seealso cref= #CanonicalCase(String) </seealso>
        public virtual IEnumerator GetPrefixedBy(string prefix, int startOffset, int stopOffset)
        {
            // Find the first node for which "prefix" prefixes KEY(node). (See the
            // implementation overview for a definition of KEY(node).) This code is
            // similar to Fetch(prefix), except that if prefix extends into the
            // middle of an edge label, that edge's child is considered a match.
            TrieNode<V> node = root;
            for (int i = startOffset; i < stopOffset; )
            {
                // Find the edge whose label starts with prefix[i].
                TrieEdge<V> edge = node.Get(prefix[i]);
                if (edge == null)
                {
                    return EMPTY_ITERATOR;
                }
                // Now check that rest of label matches
                node = edge.Child;
                string label = edge.Label;
                int j = Match(prefix, i, stopOffset, label);
                if (i + j == stopOffset)
                {
                    // a) prefix overlaps perfectly with just part of edge label
                    break;
                }
                else if (j >= 0)
                {
                    // b) prefix and label differ at some point
                    node = null;
                    break;
                }
                else
                {
                    // c) prefix overlaps perfectly with all of edge label.
                }
                i += label.Length;
            }
            // Yield all children of node, including node itself.
            if (node == null)
            {
                return EMPTY_ITERATOR;
            }
            else
            {
                return new ValueIterator(this, node);
            }
        }

        /// <summary>
        /// Returns all the (non-null) values associated with a given node and its
        /// children. (internal)
        /// </summary>
        private class ValueIterator : NodeIterator
        {
            private readonly Trie<S,V> outerInstance;

            internal ValueIterator(Trie<S, V> outerInstance, TrieNode<V> start) : base(start, false)
            {
                this.outerInstance = outerInstance;
            }

            // inherits javadoc comment
            public override object Current
            {
                get
                {
                    return ((TrieNode<V>)base.Current).Value;
                }
            }
        }

        /// <summary>
        /// Yields nothing. (internal)
        /// </summary>
        private class EmptyIterator : IEnumerator
        {
            // inherits javadoc comment
            public virtual bool MoveNext()
            {
                return false;
            }

            // inherits javadoc comment
            public virtual object Current
            {
                get
                {
                    throw new Exception();
                    //throw new NoSuchElementException();
                }
            }

            public virtual void Reset()
            {

            }
        }

        public class NodeIterator : UnmodifiableIterator
        {
            //private readonly Trie<S,V> outerInstance;

            /// <summary>
            /// Stack for DFS. Push and pop from back. The last element of stack is
            /// the next node who's value will be returned.
            /// <p>
            /// 
            /// INVARIANT: Top of stack contains the next node with not null value to
            /// pop. All other elements in stack are iterators.
            /// </summary>
            private ArrayList stack = new ArrayList(); // of Iterator of TrieNode
            private bool withNulls;

            /// <summary>
            /// Creates a new iterator that yields all the nodes of start and its
            /// children that have values (ignoring internal nodes).
            /// </summary>
            public NodeIterator(TrieNode<V> start, bool withNulls)
            {
                //this.outerInstance = outerInstance;
                this.withNulls = withNulls;
                if (withNulls || start.Value != null)
                // node has a value, push it for next
                {
                    stack.Add(start);
                }
                else
                // scan node children to find the next node
                {
                    Advance(start);
                }
            }

            // inherits javadoc comment
            public virtual bool MoveNext()
            {
                return stack.Count > 0;
            }

            // inherits javadoc comment
            public virtual object Current
            {
                get{
                    int size;
                    if ((size = stack.Count) == 0)
                    {
                        //throw new NoSuchElementException();
                    }

                    TrieNode<V> node = (TrieNode<V>)stack[size - 1];

                    stack.RemoveAt(size - 1);
                    Advance(node);
                    return node;
                }
            }

            /// <summary>
            /// Scan the tree (top-down) starting at the already visited node until
            /// finding an appropriate node with not null value for Current. Keep
            /// unvisited nodes in a stack of siblings iterators. Return either an
            /// empty stack, or a stack whose top will be the next node returned by
            /// Current.
            /// </summary>
            private void Advance(TrieNode<V> node)
            {
                IEnumerator children = node.ChildrenForward();
                while (true) // scan siblings and their children
                {
                    int size;
                    if (children.MoveNext())
                    {
                        node = (TrieNode<V>)children.Current;
                        if (children.MoveNext()) // save siblings
                        {
                            stack.Add(children);
                        }
                        // check current node and scan its sibling if necessary
                        if (withNulls || node.Value == null)
                        {
                            children = node.ChildrenForward(); // loop from there
                        }
                        else // node qualifies for Current
                        {
                            stack.Add(node);
                            return; // next node exists
                        }
                    }
                    else if ((size = stack.Count) == 0)
                    {
                        return; // no next node
                    }
                    else
                    // no more siblings, return to parent
                    {
                        children = (IEnumerator)stack[size - 1];
                        stack.RemoveAt(size - 1);
                    }
                }
            }
        }

        /// <summary>
        /// Returns a string representation of the tree state of this, i.e., the
        /// concrete state. (The version of toString commented out below returns a
        /// representation of the abstract state of this.
        /// </summary>
        public virtual string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("<root>"); //$NON-NLS-1$
            ToStringHelper(root, buf, 1);
            return buf.ToString();
        }

        /// <summary>
        /// Prints a description of the substree starting with start to buf. The
        /// printing starts with the given indent level. (internal)
        /// </summary>
        private void ToStringHelper(TrieNode<V> start, StringBuilder buf, int indent)
        {
            // Print value of node.
            if (start.Value != null)
            {
                buf.Append(" -> "); //$NON-NLS-1$
                buf.Append(start.Value.ToString());
            }
            buf.AppendLine(); //$NON-NLS-1$
            // For each child...
            for (IEnumerator iter = start.labelsForward(); iter.MoveNext(); )
            {
                // Indent child appropriately.
                for (int i = 0; i < indent; i++)
                {
                    buf.Append(" "); //$NON-NLS-1$
                }
                // Print edge.
                string label = (string)iter.Current;
                buf.Append(label);
                // Recurse to print value.
                TrieNode<V> child = start.Get(label[0]).Child;
                ToStringHelper(child, buf, indent + 1);
            }
        }
    }

    /// <summary>
    /// A node of the Trie. Each Trie has a list of children, labelled by strings.
    /// Each of these [String label, TrieNode<V> child] pairs is considered an "edge".
    /// The first character of each label must be distinct. When managing children,
    /// different implementations may trade space for time. Each node also stores an
    /// arbitrary Object value.
    /// <p>
    /// 
    /// Design note: this is a "dumb" class. It is <i>only</i> responsible for
    /// managing its value and its children. None of its operations are recursive;
    /// that is Trie's job. Nor does it deal with case.
    /// </summary>
    public class TrieNode<V>
    {
        /// <summary>
        /// The value of this node.
        /// </summary>
        private V value = default(V);

        /// <summary>
        /// The list of children. Children are stored as a sorted Vector because it
        /// is a more compact than a tree or linked lists. Insertions and deletions
        /// are more expensive, but they are rare compared to searching.
        /// <p>
        /// 
        /// INVARIANT: children are sorted by distinct first characters of edges,
        /// i.e., for all i &lt; j,<br>
        /// children[i].edge.CharAt(0) &lt; children[j].edge.CharAt(0)
        /// </summary>
        private List<TrieEdge<V>> children = new List<TrieEdge<V>>(0); // of TrieEdge

        /// <summary>
        /// Creates a trie with no children and no value.
        /// </summary>
        public TrieNode()
        {
        }

        /// <summary>
        /// Creates a trie with no children and the given value.
        /// </summary>
        public TrieNode(V value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets the value associated with this node, or null if none.
        /// </summary>
        public V Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }


        /// <summary>
        /// Get the nth child edge of this node.
        /// 
        /// @requires 0 &lt;= i &lt; children.Count
        /// </summary>
        private TrieEdge<V> Get(int i)
        {
            return children[i];
        }

        /// <summary>
        /// (internal) If exact, returns the unique i so that:
        /// children[i].getLabelStart() == c<br>
        /// If !exact, returns the largest i so that: children[i].getLabelStart()
        /// &lt;= c<br>
        /// In either case, returns -1 if no such i exists.
        /// <p>
        /// 
        /// This method uses binary search and runs in O(log N) time, where N =
        /// children.Count.<br>
        /// The standard Java binary search methods could not be used because they
        /// only return exact matches. Also, they require allocating a dummy Trie.
        /// 
        /// Example1: Search non exact c == '_' in {[0] => 'a...', [1] => 'c...'};
        /// start loop with low = 0, high = 1; middle = 0, cmiddle == 'a', c <
        /// cmiddle, high = 0 (low == 0); middle = 0, cmiddle == 'a', c < cmiddle,
        /// high = -1 (low == 0); end loop; return high == -1 (no match, insert at
        /// 0). Example2: Search non exact c == 'a' in {[0] => 'a', [1] => 'c'} start
        /// loop with low = 0, high = 1; middle = 0, cmiddle == 'a', c == cmiddle,
        /// abort loop by returning middle == 0 (exact match). Example3: Search non
        /// exact c == 'b' in {[0] => 'a...', [1] => 'c...'}; start loop with low =
        /// 0, high = 1; middle = 0, cmiddle == 'a', cmiddle < c, low = 1 (high ==
        /// 1); middle = 1, cmiddle == 'c', c < cmiddle, high = 0 (low == 1); end
        /// loop; return high == 0 (no match, insert at 1). Example4: Search non
        /// exact c == 'c' in {[0] => 'a...', [1] => 'c...'}; start loop with low =
        /// 0, high = 1; middle = 0, cmiddle == 'a', cmiddle < c, low = 1 (high ==
        /// 1); middle = 1, cmiddle == 'c', c == cmiddle, abort loop by returning
        /// middle == 1 (exact match). Example5: Search non exact c == 'd' in {[0] =>
        /// 'a...', [1] => 'c...'}; start loop with low = 0, high = 1; middle = 0,
        /// cmiddle == 'a', cmiddle < c, low = 1 (high == 1); middle = 1, cmiddle ==
        /// 'c', cmiddle < c, low = 2 (high == 1); end loop; return high == 1 (no
        /// match, insert at 2).
        /// </summary>
        private int Search(char c, bool exact)
        {
            // This code is stolen from IntSet.search.
            int low = 0;
            int high = children.Count - 1;
            while (low <= high)
            {
                int middle = (low + high) / 2;
                char cmiddle = Get(middle).LabelStart;
                if (cmiddle < c)
                {
                    low = middle + 1;
                }
                else if (c < cmiddle)
                {
                    high = middle - 1;
                }
                else
                // c == cmiddle
                {
                    return middle; // Return exact match.
                }
            }
            if (exact)
            {
                return -1; // Return no match.
            }
            return high; // Return closest *lower or equal* match. (This works!)
        }

        /// <summary>
        /// Returns the edge (at most one) whose label starts with the given
        /// character, or null if no such edge.
        /// </summary>
        public TrieEdge<V> Get(char labelStart)
        {
            int i = Search(labelStart, true);
            if (i < 0)
            {
                return null;
            }
            TrieEdge<V> ret = Get(i);
            return ret;
        }

        /// <summary>
        /// Inserts an edge with the given label to the given child to this. Keeps
        /// all edges binary sorted by their label start.
        /// 
        /// @requires label not empty.
        /// @requires for all edges E in this, label.getLabel[0] != E not already
        ///           mapped to a node.
        /// @modifies this
        /// </summary>
        public void Put(string label, TrieNode<V> child)
        {
            int i;
            // If there's a match it is the closest lower or equal one, and
            // precondition requires it to be lower, so we add the edge *after*
            // it. If there's no match, there are two cases: the Trie is empty,
            // or the closest match returned is the last edge in the list.
            if ((i = Search(label[0], false)) >= 0) // find closest match
            {
            }
            children.Insert(i + 1, new TrieEdge<V>(label, child));
        }

        /// <summary>
        /// Removes the edge (at most one) whose label starts with the given
        /// character. Returns true if any edges where actually removed.
        /// </summary>
        public bool Remove(char labelStart)
        {
            int i;
            if ((i = Search(labelStart, true)) < 0)
            {
                return false;
            }
            children.RemoveAt(i);
            return true;
        }

        /// <summary>
        /// Ensures that this's children take a minimal amount of storage. This
        /// should be called after numerous calls to Add().
        /// 
        /// @modifies this
        /// </summary>
        public void Trim()
        {
            children.TrimExcess();
        }

        /// <summary>
        /// Returns the children of this in forward order, as an iterator of
        /// TrieNode.
        /// </summary>
        public IEnumerator ChildrenForward()
        {
            return new ChildrenForwardIterator(this);
        }

        /// <summary>
        /// Maps (lambda(edge) edge.getChild) on children.iterator().
        /// </summary>
        private class ChildrenForwardIterator : UnmodifiableIterator
        {
            private readonly TrieNode<V> outerInstance;

            public ChildrenForwardIterator(TrieNode<V> outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            internal int i = 0;

            public virtual bool MoveNext()
            {
                return i < outerInstance.children.Count;
            }

            public virtual object Current
            {
                get{
                if (i < outerInstance.children.Count)
                {
                    return outerInstance.Get(i++).Child;
                }
                //throw new NoSuchElementException();
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// Returns the children of this in forward order, as an iterator of
        /// TrieNode.
        /// </summary>
        /*
         * public Iterator childrenBackward() { return new
         * ChildrenBackwardIterator(); }
         */

        /// <summary>
        /// Maps (lambda(edge) edge.getChild) on children.iteratorBackward().
        /// </summary>
        /*
         * private class ChildrenBackwardIterator extends UnmodifiableIterator { int
         * i = children.Count - 1;
         * 
         * public Boolean MoveNext() { return i >= 0; }
         * 
         * public Object Current { if (i >= 0) return Get(i--).getChild(); throw new
         * NoSuchElementException(); } }
         */

        /// <summary>
        /// Returns the labels of the children of this in forward order, as an
        /// iterator of Strings.
        /// </summary>
        public IEnumerator labelsForward()
        {
            return new LabelForwardIterator(this);
        }

        /// <summary>
        /// Maps (lambda(edge) edge.getLabel) on children.iterator()
        /// </summary>
        private class LabelForwardIterator : UnmodifiableIterator
        {
            private readonly TrieNode<V> outerInstance;

            public LabelForwardIterator(TrieNode<V> outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            internal int i = 0;

            public virtual bool MoveNext()
            {
                return i < outerInstance.children.Count;
            }

            public virtual object Current
            {
                get{
                if (i < outerInstance.children.Count)
                {
                    return outerInstance.Get(i++).Label;
                }
                throw new Exception(); // NoSuchElementException();
                }
            }
        }

        /// <summary>
        /// Returns the labels of the children of this in backward order, as an
        /// iterator of Strings.
        /// </summary>
        /*
         * public Iterator labelsBackward() { return new LabelBackwardIterator(); }
         */

        /// <summary>
        /// Maps (lambda(edge) edge.getLabel) on children.iteratorBackward()
        /// </summary>
        /*
         * private class LabelBackwardIterator extends UnmodifiableIterator { int i =
         * children.Count - 1;
         * 
         * public Boolean MoveNext() { return i >= 0; }
         * 
         * public Object Current { if (i >= 0) return Get(i--).getLabel(); throw new
         * NoSuchElementException(); } }
         */

        // inherits javadoc comment.
        public string ToString()
        {
            object val = Value;
            if (val != null)
            {
                return val.ToString();
            }
            return "NULL"; //$NON-NLS-1$
        }

        /// <summary>
        /// Unit test.
        /// </summary>
        /// <seealso cref= TrieNodeTest </seealso>
    }

    /// <summary>
    /// A labelled edge, i.e., a String label and a TrieNode<V> endpoint.
    /// </summary>
    public class TrieEdge<V>
    {
        private string label;
        private TrieNode<V> child;

        /// <summary>
        /// @requires label.Count > 0
        /// @requires child != null
        /// </summary>
        internal TrieEdge(string label, TrieNode<V> child)
        {
            this.label = label;
            this.child = child;
        }

        public string Label
        {
            get
            {
                return label;
            }
        }

        /// <summary>
        /// Returns the first character of the label, i.e., getLabel().CharAt(0).
        /// </summary>
        public char LabelStart
        {
            get
            {
                // You could store this char as an optimization if needed.
                return label[0];
            }
        }

        public TrieNode<V> Child
        {
            get
            {
                return child;
            }
        }

    }
}
