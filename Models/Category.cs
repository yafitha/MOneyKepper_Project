using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string PictureName { get; set; }
        public int TypeID { get; set; }
        public bool IsActive { get; set; }
        public bool IsParent { get; set; }
        public Nullable<int> ParentID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    
    public Category(int id, string name, int typeID, string pictureName, bool isParent, int? parentID = null, bool isActive = true)
        {
            this.ID = id;
            this.TypeID = typeID;
            this.Name = name;
            this.PictureName = pictureName;
            this.IsParent = isParent;
            this.ParentID = parentID;
            this.IsActive = isActive;
        }
    }
}
