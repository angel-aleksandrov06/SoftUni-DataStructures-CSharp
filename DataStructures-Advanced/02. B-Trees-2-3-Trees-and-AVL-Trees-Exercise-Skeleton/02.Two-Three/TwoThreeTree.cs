namespace _02.Two_Three
{
    using System;
    using System.Text;

    public class TwoThreeTree<T> where T : IComparable<T>
    {
        private TreeNode<T> root;

        public void Insert(T key)
        {
            this.root = Insert(this.root, key);
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            RecursivePrint(this.root, sb);
            return sb.ToString();
        }

        private void RecursivePrint(TreeNode<T> node, StringBuilder sb)
        {
            if (node == null)
            {
                return;
            }

            if (node.LeftKey != null)
            {
                sb.Append(node.LeftKey).Append(" ");
            }

            if (node.RightKey != null)
            {
                sb.Append(node.RightKey).Append(Environment.NewLine);
            }
            else
            {
                sb.Append(Environment.NewLine);
            }

            if (node.IsTwoNode())
            {
                RecursivePrint(node.LeftChild, sb);
                RecursivePrint(node.MiddleChild, sb);
            }
            else if (node.IsThreeNode())
            {
                RecursivePrint(node.LeftChild, sb);
                RecursivePrint(node.MiddleChild, sb);
                RecursivePrint(node.RightChild, sb);
            }
        }

        private TreeNode<T> Insert(TreeNode<T> node, T key)
        {
            if (node == null)
            {
                return new TreeNode<T>(key);
            }

            TreeNode<T> returnNode;

            if (node.IsLeaf())
            {
                return Merge(node, new TreeNode<T>(key));
            }

            if (key.CompareTo(node.LeftKey) < 0)
            {
                returnNode = this.Insert(node.LeftChild, key);
                if (returnNode == node.LeftChild)
                {
                    return node;
                }
                else
                {
                    return Merge(node, returnNode);
                }
            }
            else if (node.IsTwoNode() || key.CompareTo(node.RightKey) < 0)
            {
                returnNode = this.Insert(node.MiddleChild, key);
                if (returnNode == node.MiddleChild)
                {
                    return node;
                }
                else
                {
                    return Merge(node, returnNode);
                }
            }
            else
            {
                returnNode = this.Insert(node.RightChild, key);
                if (returnNode == node.RightChild)
                {
                    return node;
                }
                else
                {
                    return Merge(node, returnNode);
                }
            }
        }

        private TreeNode<T> Merge(TreeNode<T> currentNode, TreeNode<T> newNode)
        {
            if (currentNode.RightKey == null)
            {
                if (currentNode.LeftKey.CompareTo(newNode.LeftKey) < 0)
                {
                    currentNode.RightKey = newNode.LeftKey;
                    currentNode.MiddleChild = newNode.LeftChild;
                    currentNode.RightChild = newNode.MiddleChild;
                }
                else
                {
                    currentNode.RightKey = currentNode.LeftKey;
                    currentNode.RightChild = currentNode.MiddleChild;
                    currentNode.LeftKey = newNode.LeftKey;
                    currentNode.MiddleChild = newNode.MiddleChild;
                }
                return currentNode;
            }
            else if (currentNode.LeftKey.CompareTo(newNode.LeftKey) >= 0)
            {
                TreeNode<T> mergeNode = new TreeNode<T>(currentNode.LeftKey)
                {
                    LeftChild = newNode,
                    MiddleChild = currentNode,
                };

                newNode.LeftChild = currentNode.LeftChild;
                currentNode.LeftChild = currentNode.MiddleChild;
                currentNode.MiddleChild = currentNode.RightChild;
                currentNode.RightChild = null;
                currentNode.LeftKey = currentNode.RightKey;
                currentNode.RightKey = default;
                return mergeNode;
            }
            else if (currentNode.RightKey.CompareTo(newNode.LeftKey) >= 0)
            {
                newNode.MiddleChild = new TreeNode<T>(currentNode.RightKey)
                {
                    LeftChild = newNode.MiddleChild,
                    MiddleChild = currentNode.RightChild,
                };

                newNode.LeftChild = currentNode;
                currentNode.RightKey = default;
                currentNode.RightChild = null;
                return newNode;
            }
            else
            {
                TreeNode<T> mergedNode = new TreeNode<T>(currentNode.RightKey)
                {
                    LeftChild = currentNode,
                    MiddleChild = newNode
                };

                newNode.LeftChild = currentNode.RightChild;
                currentNode.RightChild = null;
                currentNode.RightKey = default;
                return mergedNode;
            }
        }
    }
}
