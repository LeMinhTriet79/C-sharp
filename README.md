# C-sharp
# CẨM NANG SINH TỒN C# & .NET CORE BACKEND
### Dành cho lập trình viên Java chuyển sang hệ sinh thái .NET

> Ngữ cảnh xuyên suốt: hệ thống E-commerce Backend (Order, User, Payment, Product).
> Mọi khái niệm đều đi kèm code chạy được và dòng `// Output:` để bạn tự kiểm chứng.

---

## PHẦN 1: C# CƠ BẢN & TƯ DUY OOP KIỂU .NET

### 1.1. Tham trị và Tham chiếu: `struct` vs `class`

Java chỉ có một loại "vật chứa" thật sự là class (reference type), còn các kiểu nguyên thủy (`int`, `double`...) chỉ là ngoại lệ được xử lý riêng. C# đối xử công bằng hơn: bạn có thể tự định nghĩa kiểu giá trị (`struct`) song song với kiểu tham chiếu (`class`). Đây là khác biệt nền tảng đầu tiên bạn phải khắc cốt ghi tâm.

**Bản chất:**
- `class` → cấp phát trên **Heap**, biến chỉ giữ một tham chiếu (địa chỉ). Gán `b = a` nghĩa là `b` và `a` trỏ chung một object.
- `struct` → cấp phát trên **Stack** (khi là biến cục bộ), biến giữ **toàn bộ dữ liệu**. Gán `b = a` nghĩa là **sao chép** toàn bộ giá trị, `b` và `a` độc lập nhau.

Trong E-commerce, một lựa chọn kinh điển: `Money` (số tiền, tọa độ) nên là `struct` vì nó là một giá trị bất biến, nhỏ gọn; còn `Order`, `Product` nên là `class` vì chúng có định danh (identity) và vòng đời riêng cần theo dõi.

```csharp
using System;

// STRUCT: kiểu giá trị — mô phỏng một khoản tiền
public struct Money
{
    public decimal Amount;
    public string Currency;

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
}

// CLASS: kiểu tham chiếu — mô phỏng một đơn hàng
public class Order
{
    public string OrderId;
    public decimal TotalAmount;

    public Order(string orderId, decimal totalAmount)
    {
        OrderId = orderId;
        TotalAmount = totalAmount;
    }
}

public class Program
{
    // Hàm nhận struct: nhận BẢN SAO
    static void ApplyDiscountToMoney(Money m)
    {
        m.Amount -= 10; // chỉ sửa trên bản sao cục bộ
    }

    // Hàm nhận class: nhận THAM CHIẾU tới object gốc
    static void ApplyDiscountToOrder(Order o)
    {
        o.TotalAmount -= 10; // sửa trực tiếp trên object gốc
    }

    public static void Main()
    {
        Money price = new Money(100, "VND");
        ApplyDiscountToMoney(price);
        Console.WriteLine($"Money.Amount sau khi gọi hàm: {price.Amount}");
        // Output: Money.Amount sau khi gọi hàm: 100

        Order order = new Order("ORD-001", 100);
        ApplyDiscountToOrder(order);
        Console.WriteLine($"Order.TotalAmount sau khi gọi hàm: {order.TotalAmount}");
        // Output: Order.TotalAmount sau khi gọi hàm: 90
    }
}
```

**Điểm mấu chốt cho phỏng vấn:** `struct` không hỗ trợ kế thừa (trừ interface), luôn có constructor mặc định ngầm định, và nếu struct quá lớn (>16 byte theo khuyến nghị Microsoft) thì việc sao chép liên tục sẽ **phản tác dụng về hiệu năng** — lúc đó dùng `class` mới đúng.

---

### 1.2. Properties (get/set): Tại sao C# không cần "Getter/Setter" dài dòng như Java?

Trong Java, chuẩn Bean bắt bạn viết:

```java
private double price;
public double getPrice() { return price; }
public void setPrice(double price) { this.price = price; }
```

C# coi việc này là cú pháp **quá tải trọng** cho một nhu cầu quá phổ biến, nên sinh ra **Property** — một thành phần ngôn ngữ lai giữa field và method, cho phép bạn viết field nhưng bên dưới compiler tự sinh ra get/set method (`get_Price()`, `set_Price()` trong IL).

```csharp
using System;

public class Product
{
    // Auto-implemented property: compiler tự sinh backing field ẩn
    public string Name { get; set; }

    // Property có validation: buộc phải viết get/set tường minh
    private decimal _price;
    public decimal Price
    {
        get => _price;
        set
        {
            if (value < 0)
                throw new ArgumentException("Giá sản phẩm không được âm.");
            _price = value;
        }
    }

    // Init-only property (C# 9+): chỉ gán được lúc khởi tạo object
    public string Sku { get; init; }

    // Property chỉ đọc, tính toán từ field khác (computed property)
    public string DisplayName => $"{Name} (SKU: {Sku})";
}

public class Program
{
    public static void Main()
    {
        var product = new Product { Name = "Bàn phím cơ", Sku = "KB-001" };
        product.Price = 500000;

        Console.WriteLine(product.DisplayName);
        // Output: Bàn phím cơ (SKU: KB-001)

        try
        {
            product.Price = -100;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Lỗi: {ex.Message}");
            // Output: Lỗi: Giá sản phẩm không được âm.
        }
    }
}
```

**So sánh tư duy:** Java tách biệt rạch ròi "field nội bộ" và "method public" — bạn luôn thấy rõ đâu là API. C# gộp chúng lại thành Property để code gọn hơn, nhưng đổi lại: khi đọc code C#, bạn phải nhớ rằng `product.Price = -100` **thực chất là một lời gọi method** (`set_Price`), không phải phép gán field trần trụi — vì vậy nó hoàn toàn có thể ném exception.

---

### 1.3. OOP trong C#: `virtual` và `override` — Đa hình qua hệ thống tính phí vận chuyển

Java mặc định **mọi method non-static, non-private đều có thể override** (trừ khi đánh dấu `final`). C# đi ngược lại: mặc định method **không thể override**, bạn phải **chủ động khai báo** `virtual` ở lớp cha thì lớp con mới được phép `override`. Đây là triết lý "an toàn trước, mở rộng sau" — thiết kế lớp cha phải cân nhắc kỹ đâu là điểm cho phép hành vi thay đổi.

```csharp
using System;
using System.Collections.Generic;

public abstract class ShippingFeeCalculator
{
    // virtual: cho phép lớp con ghi đè
    public virtual decimal Calculate(decimal orderValue, double distanceKm)
    {
        return 15000; // Phí cơ bản mặc định
    }
}

public class StandardShipping : ShippingFeeCalculator
{
    public override decimal Calculate(decimal orderValue, double distanceKm)
    {
        return (decimal)distanceKm * 2000;
    }
}

public class ExpressShipping : ShippingFeeCalculator
{
    // Gọi lại logic của lớp cha bằng "base", rồi cộng thêm phụ phí
    public override decimal Calculate(decimal orderValue, double distanceKm)
    {
        decimal baseFee = base.Calculate(orderValue, distanceKm);
        return baseFee + 25000; // phụ phí giao nhanh
    }
}

public class FreeShippingOverThreshold : ShippingFeeCalculator
{
    public override decimal Calculate(decimal orderValue, double distanceKm)
    {
        return orderValue >= 500000 ? 0 : 20000;
    }
}

public class Program
{
    public static void Main()
    {
        // Tính đa hình: cùng một biến kiểu cha, nhưng gọi ra hành vi khác nhau
        List<ShippingFeeCalculator> strategies = new List<ShippingFeeCalculator>
        {
            new StandardShipping(),
            new ExpressShipping(),
            new FreeShippingOverThreshold()
        };

        decimal orderValue = 600000;
        double distance = 5.0;

        foreach (var strategy in strategies)
        {
            decimal fee = strategy.Calculate(orderValue, distance);
            Console.WriteLine($"{strategy.GetType().Name}: {fee:N0} VND");
        }
        // Output: StandardShipping: 10,000 VND
        // Output: ExpressShipping: 35,000 VND
        // Output: FreeShippingOverThreshold: 0 VND
    }
}
```

**Bẫy thường gặp:** nếu bạn quên từ khóa `override` và chỉ viết lại method trùng tên trùng signature ở lớp con, C# sẽ hiểu đó là **method ẩn (`new` ngầm định)** chứ không phải ghi đè — lúc gọi qua biến kiểu cha, hành vi lớp cha sẽ chạy chứ không phải lớp con. Đây là lỗi runtime rất khó phát hiện nếu không hiểu rõ cơ chế này.

---

### GÓC PHỎNG VẤN — PHẦN 1

**1. `struct` và `class` khác nhau ở đâu, và khi nào bạn chọn `struct` thay vì `class`?**
> Trả lời sắc bén: Khác biệt cốt lõi là nơi cấp phát bộ nhớ (Stack vs Heap) và ngữ nghĩa gán biến (copy-by-value vs copy-by-reference). Chọn `struct` khi: dữ liệu nhỏ gọn (khuyến nghị dưới 16 byte), bất biến (immutable), không cần identity, và được tạo/hủy với tần suất cao (để giảm áp lực GC). Ví dụ điển hình: `Point`, `Money`, `DateTime` (chính `DateTime` trong .NET cũng là struct).

**2. Property trong C# khác gì so với field public thông thường? Tại sao không dùng field public cho gọn?**
> Trả lời sắc bén: Property cho phép chèn logic (validation, lazy loading, thông báo thay đổi) mà không phá vỡ **binary compatibility** — nếu sau này bạn đổi field public thành property, mọi assembly đã compile tham chiếu tới field đó sẽ lỗi. Property cũng là nền tảng để tương thích với data-binding (WPF, Blazor) và serialization framework như System.Text.Json.

**3. Nếu không có từ khóa `virtual` ở lớp cha, chuyện gì xảy ra khi lớp con dùng `override`?**
> Trả lời sắc bén: Compiler sẽ báo lỗi biên dịch ngay lập tức (`cannot override inherited member because it is not marked virtual, abstract, or override`). Đây là chủ đích thiết kế của C#: buộc lập trình viên lớp cha phải khai báo tường minh "điểm mở rộng" nào được phép thay đổi, tránh tình trạng override tràn lan phá vỡ tính đóng gói như trong Java.

---

## PHẦN 2: COLLECTIONS, DELEGATE & LINQ (VŨ KHÍ TỐI THƯỢNG CỦA C#)

### 2.1. Collections: Khi nào dùng `List<T>`, khi nào dùng `Dictionary<TKey, TValue>`

Nguyên tắc chọn lựa rất thực dụng: bạn cần **duyệt tuần tự theo thứ tự chèn** và không quan tâm tra cứu nhanh → `List<T>`. Bạn cần **tra cứu theo khóa với độ phức tạp O(1)** → `Dictionary<TKey, TValue>`.

```csharp
using System;
using System.Collections.Generic;

public class Order
{
    public int OrderId { get; set; }
    public string Status { get; set; }
    public decimal Total { get; set; }
}

public class Program
{
    public static void Main()
    {
        // List<T>: danh sách đơn hàng theo thứ tự tạo
        List<Order> orders = new List<Order>
        {
            new Order { OrderId = 1, Status = "Pending", Total = 200000 },
            new Order { OrderId = 2, Status = "Completed", Total = 550000 },
            new Order { OrderId = 3, Status = "Completed", Total = 120000 }
        };

        Console.WriteLine($"Đơn hàng đầu tiên trong danh sách: #{orders[0].OrderId}");
        // Output: Đơn hàng đầu tiên trong danh sách: #1

        // Dictionary<TKey, TValue>: tra cứu nhanh đơn hàng theo OrderId
        Dictionary<int, Order> orderLookup = new Dictionary<int, Order>();
        foreach (var order in orders)
        {
            orderLookup[order.OrderId] = order;
        }

        // Tra cứu O(1) thay vì phải duyệt cả List O(n)
        if (orderLookup.TryGetValue(2, out Order foundOrder))
        {
            Console.WriteLine($"Tìm thấy đơn #2, trạng thái: {foundOrder.Status}");
        }
        // Output: Tìm thấy đơn #2, trạng thái: Completed
    }
}
```

**Cảnh báo hiệu năng:** dùng `List<T>.Find()` hoặc `.FirstOrDefault()` để tra cứu theo ID trên tập dữ liệu lớn (hàng chục nghìn đơn hàng trở lên) là **anti-pattern** kinh điển của dev mới — độ phức tạp O(n) mỗi lần tra cứu. Nếu tra cứu theo khóa là thao tác lặp lại nhiều lần, hãy build sẵn `Dictionary` một lần.

---

### 2.2. Delegate và Action/Func: Bản chất đơn giản nhất có thể

Nói thẳng: **Delegate là một "biến" giữ tham chiếu tới một hoặc nhiều method**, giống như con trỏ hàm trong C nhưng an toàn kiểu (type-safe) và hướng đối tượng. `Action` và `Func` là hai delegate **có sẵn trong .NET** để bạn khỏi phải tự khai báo delegate riêng cho từng trường hợp:

- `Action<T>` → gói một method **không trả về giá trị** (void).
- `Func<T, TResult>` → gói một method **có trả về giá trị**, tham số cuối cùng luôn là kiểu trả về.

```csharp
using System;
using System.Collections.Generic;

public class Order
{
    public int OrderId { get; set; }
    public decimal Total { get; set; }
    public string Category { get; set; }
}

public class Program
{
    public static void Main()
    {
        List<Order> orders = new List<Order>
        {
            new Order { OrderId = 1, Total = 200000, Category = "Electronics" },
            new Order { OrderId = 2, Total = 550000, Category = "Fashion" }
        };

        // Func<Order, decimal>: nhận vào 1 Order, trả ra decimal (số tiền giảm giá)
        Func<Order, decimal> vipDiscountRule = (order) => order.Total * 0.1m;
        Func<Order, decimal> newUserDiscountRule = (order) => 20000m;

        // Truyền delegate như tham số — đây chính là sức mạnh "hàm là công dân hạng nhất"
        decimal ApplyDiscount(Order order, Func<Order, decimal> discountRule)
        {
            decimal discount = discountRule(order);
            return order.Total - discount;
        }

        decimal finalPrice = ApplyDiscount(orders[0], vipDiscountRule);
        Console.WriteLine($"Giá cuối cho đơn #1 (khách VIP): {finalPrice:N0} VND");
        // Output: Giá cuối cho đơn #1 (khách VIP): 180,000 VND

        // Action<T>: chỉ thực thi hành động, không trả kết quả — ví dụ log/ghi nhận
        Action<Order> logOrder = (order) =>
            Console.WriteLine($"[LOG] Đơn hàng #{order.OrderId} thuộc danh mục {order.Category}");

        logOrder(orders[1]);
        // Output: [LOG] Đơn hàng #2 thuộc danh mục Fashion
    }
}
```

**Điểm khác biệt với Java:** Java không có delegate thực thụ — Java 8 mô phỏng khái niệm tương đương bằng **Functional Interface** (`Function<T,R>`, `Consumer<T>`) kết hợp lambda. Về bản chất hai bên giải quyết cùng một vấn đề (truyền hành vi như dữ liệu), nhưng C# có cú pháp delegate là **first-class citizen** của ngôn ngữ từ C# 1.0, còn Java phải chờ tới Java 8 mới có lambda dựa trên interface.

---

### 2.3. LINQ: So sánh `foreach` truyền thống và LINQ

LINQ là công cụ giúp bạn viết truy vấn dữ liệu **khai báo (declarative)** thay vì **mệnh lệnh (imperative)**. Bạn mô tả "tôi muốn gì" thay vì "làm từng bước như thế nào".

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

public class Order
{
    public int OrderId { get; set; }
    public string Status { get; set; }
    public string Category { get; set; }
    public decimal Total { get; set; }
}

public class Program
{
    public static void Main()
    {
        List<Order> orders = new List<Order>
        {
            new Order { OrderId = 1, Status = "Completed", Category = "Electronics", Total = 500000 },
            new Order { OrderId = 2, Status = "Pending",   Category = "Fashion",     Total = 150000 },
            new Order { OrderId = 3, Status = "Completed", Category = "Fashion",     Total = 300000 },
            new Order { OrderId = 4, Status = "Cancelled",  Category = "Electronics", Total = 700000 },
            new Order { OrderId = 5, Status = "Completed", Category = "Electronics", Total = 250000 }
        };

        // ==== CÁCH 1: foreach truyền thống ====
        List<Order> completedOrdersOldWay = new List<Order>();
        foreach (var order in orders)
        {
            if (order.Status == "Completed")
            {
                completedOrdersOldWay.Add(order);
            }
        }
        decimal totalRevenueOldWay = 0;
        foreach (var order in completedOrdersOldWay)
        {
            totalRevenueOldWay += order.Total;
        }
        Console.WriteLine($"[foreach] Tổng doanh thu đơn Completed: {totalRevenueOldWay:N0} VND");
        // Output: [foreach] Tổng doanh thu đơn Completed: 1,050,000 VND

        // ==== CÁCH 2: LINQ — lọc + tính tổng doanh thu ====
        decimal totalRevenueLinq = orders
            .Where(o => o.Status == "Completed")
            .Sum(o => o.Total);
        Console.WriteLine($"[LINQ] Tổng doanh thu đơn Completed: {totalRevenueLinq:N0} VND");
        // Output: [LINQ] Tổng doanh thu đơn Completed: 1,050,000 VND

        // ==== LINQ nâng cao: gom nhóm theo danh mục và tính doanh thu mỗi nhóm ====
        var revenueByCategory = orders
            .Where(o => o.Status == "Completed")
            .GroupBy(o => o.Category)
            .Select(g => new { Category = g.Key, TotalRevenue = g.Sum(o => o.Total) })
            .OrderByDescending(x => x.TotalRevenue);

        foreach (var group in revenueByCategory)
        {
            Console.WriteLine($"{group.Category}: {group.TotalRevenue:N0} VND");
        }
        // Output: Electronics: 750,000 VND
        // Output: Fashion: 300,000 VND
    }
}
```

**Nhận định thẳng thắn:** LINQ không "nhanh hơn" `foreach` về CPU (thực chất bên dưới LINQ vẫn sinh ra vòng lặp, cộng thêm chi phí gọi delegate và tạo iterator) — điểm mạnh của nó là **khả năng đọc hiểu (readability)** và **khả năng compose** nhiều điều kiện liên tiếp mà không tạo biến trung gian. Với dữ liệu cực lớn và cần tối ưu tuyệt đối hiệu năng, đôi khi `foreach` thủ công (tránh overhead của iterator/delegate) vẫn được ưu tiên trong các hệ thống hiệu năng cao (high-frequency trading, real-time processing).

---

### GÓC PHỎNG VẤN — PHẦN 2

**1. LINQ sử dụng "Deferred Execution" (thực thi trì hoãn) — điều này nghĩa là gì và nó có thể gây ra bug gì?**
> Trả lời sắc bén: Hầu hết LINQ method (`Where`, `Select`, `OrderBy`...) không thực thi ngay khi bạn viết dòng lệnh, mà chỉ xây dựng một "kế hoạch truy vấn". Truy vấn chỉ thực sự chạy khi bạn duyệt qua nó (`foreach`, `.ToList()`, `.Count()`...). Bug kinh điển: nếu bạn thay đổi collection nguồn sau khi định nghĩa query nhưng trước khi duyệt, kết quả sẽ phản ánh dữ liệu **tại thời điểm duyệt**, không phải tại thời điểm viết query — gây ra kết quả không như mong đợi nếu dev không hiểu rõ.

**2. Sự khác biệt giữa `Action`, `Func` và `Predicate<T>` là gì?**
> Trả lời sắc bén: Cả ba đều là delegate có sẵn. `Action<T>` không trả về giá trị. `Func<T, TResult>` trả về giá trị kiểu `TResult`. `Predicate<T>` là trường hợp đặc biệt tương đương `Func<T, bool>`, tồn tại chủ yếu vì lý do lịch sử (xuất hiện trước Func trong .NET Framework 2.0) và được dùng trong các method như `List<T>.Find()`.

**3. `Dictionary<TKey, TValue>` có đảm bảo giữ thứ tự chèn khi duyệt (enumerate) không?**
> Trả lời sắc bén: Về mặt tài liệu chính thức của Microsoft, **không hề đảm bảo** thứ tự — dù trên thực tế implementation hiện tại thường giữ thứ tự chèn cho tới khi có phần tử bị xóa (tạo "lỗ trống" được tái sử dụng). Không bao giờ được dựa vào hành vi ngầm định này trong code production; nếu cần vừa tra cứu nhanh vừa giữ thứ tự, hãy dùng cấu trúc kết hợp như `List<KeyValuePair<TKey,TValue>>` hoặc `OrderedDictionary`.

---

## PHẦN 3: BẤT ĐỒNG BỘ VỚI ASYNC/AWAIT (ĐẶC SẢN .NET)

### 3.1. Tại sao Backend cần xử lý bất đồng bộ?

Một Web API xử lý hàng nghìn request đồng thời, nhưng mỗi request thường dành phần lớn thời gian **chờ đợi I/O** (gọi database, gọi API bên thứ ba, đọc file) chứ không thực sự "tính toán". Nếu xử lý đồng bộ, một Thread bị **khóa cứng (blocked)** trong suốt thời gian chờ đó — trong khi Thread Pool của server chỉ có giới hạn, dẫn tới hết thread khả dụng và server "sập" dưới tải cao dù CPU gần như rảnh rỗi.

`async`/`await` giải quyết vấn đề này bằng cách: khi gặp một tác vụ I/O (đánh dấu bằng `await`), Thread hiện tại được **trả lại Thread Pool** để phục vụ request khác, thay vì đứng chờ. Khi tác vụ I/O hoàn tất, một Thread (có thể khác) sẽ tiếp tục thực thi phần còn lại của method.

**Bản chất kỹ thuật:** `async` chỉ là một "nhãn dán" cho compiler biết method này chứa `await` và cần được biến đổi thành một **State Machine** (máy trạng thái) — nó tự nó không tạo ra bất đồng bộ. `await` mới là từ khóa thực sự "nhường quyền điều khiển" lại cho caller trong lúc chờ `Task` hoàn thành.

```csharp
using System;
using System.Threading.Tasks;

public class UserService
{
    // Giả lập gọi database mất 1 giây
    public async Task<string> GetUserNameAsync(int userId)
    {
        await Task.Delay(1000); // mô phỏng I/O — KHÔNG chặn thread
        return $"User-{userId}";
    }
}

public class OrderService
{
    // Giả lập gọi database mất 1.5 giây
    public async Task<decimal> GetOrderTotalAsync(int orderId)
    {
        await Task.Delay(1500);
        return 350000m;
    }
}

public class Program
{
    public static async Task Main()
    {
        var userService = new UserService();
        var orderService = new OrderService();

        // ==== CÁCH 1: Tuần tự (chờ lần lượt) ====
        var stopwatch1 = System.Diagnostics.Stopwatch.StartNew();
        string userNameSeq = await userService.GetUserNameAsync(1);
        decimal totalSeq = await orderService.GetOrderTotalAsync(100);
        stopwatch1.Stop();
        Console.WriteLine($"[Tuần tự] {userNameSeq}, {totalSeq:N0} VND — mất {stopwatch1.ElapsedMilliseconds}ms");
        // Output: [Tuần tự] User-1, 350,000 VND — mất ~2500ms

        // ==== CÁCH 2: Song song (chạy đồng thời, chờ cả hai xong) ====
        var stopwatch2 = System.Diagnostics.Stopwatch.StartNew();
        Task<string> userTask = userService.GetUserNameAsync(1);
        Task<decimal> orderTask = orderService.GetOrderTotalAsync(100);
        await Task.WhenAll(userTask, orderTask);
        stopwatch2.Stop();
        Console.WriteLine($"[Song song] {userTask.Result}, {orderTask.Result:N0} VND — mất {stopwatch2.ElapsedMilliseconds}ms");
        // Output: [Song song] User-1, 350,000 VND — mất ~1500ms
    }
}
```

Chú ý sự khác biệt thời gian: cách 1 mất tổng ~2500ms (1000 + 1500), cách 2 chỉ mất ~1500ms (thời gian của tác vụ lâu nhất) vì hai tác vụ độc lập được khởi chạy song song rồi mới `await` gộp lại.

### 3.2. Lỗi kinh điển: Deadlock khi dùng `.Result` hoặc `.Wait()`

Đây là lỗi mà gần như 100% dev mới học .NET Core từng dính phải ít nhất một lần, đặc biệt khi làm việc với ASP.NET (Framework cũ) hoặc UI framework có `SynchronizationContext`.

```csharp
// ĐOẠN CODE NÀY MINH HỌA NGUYÊN NHÂN GÂY DEADLOCK
// (Trên ASP.NET Framework/WPF có SynchronizationContext — .NET Core Console không bị ảnh hưởng
//  vì không có SynchronizationContext, nhưng đây VẪN LÀ ANTI-PATTERN cần tránh tuyệt đối)

public class PaymentService
{
    public async Task<bool> ProcessPaymentAsync(decimal amount)
    {
        await Task.Delay(500); // mô phỏng gọi cổng thanh toán
        return true;
    }

    // SAI: gọi .Result trên một async method từ context đồng bộ
    public bool ProcessPaymentBlocking(decimal amount)
    {
        // .Result buộc thread hiện tại PHẢI CHỜ, đồng thời (trên context có
        // SynchronizationContext) nó cũng giữ luôn "quyền" để tiếp tục method async kia
        // -> hai bên chờ nhau vô thời hạn -> DEADLOCK
        return ProcessPaymentAsync(amount).Result;
    }

    // ĐÚNG: async/await xuyên suốt, không "chặn" thread ở đâu cả
    public async Task<bool> ProcessPaymentCorrectAsync(decimal amount)
    {
        return await ProcessPaymentAsync(amount);
    }
}

public class Program
{
    public static async Task Main()
    {
        var paymentService = new PaymentService();

        bool result = await paymentService.ProcessPaymentCorrectAsync(100000);
        Console.WriteLine($"Thanh toán thành công: {result}");
        // Output: Thanh toán thành công: True
    }
}
```

**Quy tắc sống còn:** "Async all the way" — một khi bạn đã bắt đầu dùng `async`/`await` ở tầng thấp nhất (ví dụ tầng Repository gọi database), hãy để nó `async` xuyên suốt tất cả các tầng phía trên (Service, Controller). Đừng bao giờ "bọc ngược" một async method bằng `.Result` hay `.Wait()` chỉ để nó trông có vẻ đồng bộ.

---

### GÓC PHỎNG VẤN — PHẦN 3

**1. `Task` và `Task<T>` khác nhau ở điểm nào? Còn `ValueTask<T>` dùng khi nào?**
> Trả lời sắc bén: `Task` đại diện một thao tác bất đồng bộ không trả về giá trị (tương đương `void` nhưng bất đồng bộ), `Task<T>` trả về giá trị kiểu `T` sau khi hoàn thành. `ValueTask<T>` là kiểu struct được thêm để tối ưu cho trường hợp kết quả **thường có sẵn ngay lập tức** (ví dụ đọc từ cache trong bộ nhớ) — tránh chi phí cấp phát một `Task` object trên heap ở những đường dẫn hot-path chạy đồng bộ. `ValueTask` không nên `await` nhiều lần hay lưu lại để dùng sau, khác với `Task`.

**2. `ConfigureAwait(false)` dùng để làm gì, và trong ASP.NET Core hiện đại có còn cần dùng không?**
> Trả lời sắc bén: `ConfigureAwait(false)` báo cho runtime biết: sau khi `await` xong, **không cần** quay lại đúng `SynchronizationContext` ban đầu để tiếp tục thực thi — giúp tránh deadlock và giảm overhead chuyển context, thường dùng trong thư viện (library code) không biết trước môi trường gọi nó. Trong ASP.NET Core, framework **không có SynchronizationContext** như ASP.NET Framework cũ, nên về mặt tránh deadlock nó không còn bắt buộc như trước, nhưng vẫn được khuyến nghị dùng trong code thư viện dùng chung để giảm chi phí và giữ thói quen tốt.

**3. Nếu quên `await` trước một method `async` (`Task`), chuyện gì xảy ra?**
> Trả lời sắc bén: Method vẫn được gọi và bắt đầu chạy, nhưng caller **không chờ nó hoàn thành** ("fire-and-forget") — code tiếp tục chạy tiếp ngay lập tức. Nếu bên trong method đó ném exception, exception sẽ **không được caller bắt được** (dễ gây crash ngầm hoặc lỗi bị nuốt mất), và nếu method thao tác trên dữ liệu chia sẻ, có thể gây ra race condition. Compiler sẽ cảnh báo (warning CS4014) nhưng không chặn biên dịch.

---

## PHẦN 4: DEPENDENCY INJECTION & .NET CORE WEB API

### 4.1. Dependency Injection: Vòng đời Service (`AddTransient`, `AddScoped`, `AddSingleton`)

.NET Core có một **DI Container tích hợp sẵn** (built-in IoC container) — không cần cài thêm Spring hay bất kỳ thư viện ngoài nào như trong hệ sinh thái Java (Spring Boot). Khi đăng ký một service, bạn phải chọn **vòng đời (lifetime)**, đây là điểm khác biệt gây nhầm lẫn nhiều nhất với dev mới:

| Lifetime | Ý nghĩa | Ví dụ dùng trong E-commerce |
|---|---|---|
| `AddTransient` | Tạo **instance mới mỗi lần** được inject | Service tính toán không giữ state (`ShippingFeeCalculator`) |
| `AddScoped` | Tạo **1 instance duy nhất cho mỗi HTTP Request**, tái sử dụng trong suốt request đó | `DbContext`, `OrderService` cần dùng chung 1 transaction |
| `AddSingleton` | Tạo **1 instance duy nhất cho toàn bộ vòng đời ứng dụng** | Cấu hình (`IConfiguration`), cache trong bộ nhớ, logger |

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;

// Interface đại diện một dịch vụ ghi log — Singleton, dùng chung toàn app
public interface ILoggerService
{
    Guid InstanceId { get; }
    void Log(string message);
}

public class LoggerService : ILoggerService
{
    public Guid InstanceId { get; } = Guid.NewGuid();
    public void Log(string message) => Console.WriteLine($"[LOG-{InstanceId.ToString().Substring(0, 4)}] {message}");
}

// Service mô phỏng theo từng "request" — Scoped
public interface IOrderContextService
{
    Guid InstanceId { get; }
}

public class OrderContextService : IOrderContextService
{
    public Guid InstanceId { get; } = Guid.NewGuid();
}

// Service tính phí — Transient, không giữ state, tạo mới thoải mái
public interface IShippingCalculator
{
    Guid InstanceId { get; }
}

public class ShippingCalculator : IShippingCalculator
{
    public Guid InstanceId { get; } = Guid.NewGuid();
}

public class Program
{
    // Mô phỏng xử lý một HTTP Request bằng một Scope riêng
    static void SimulateHttpRequest(IServiceProvider rootProvider, int requestNumber)
    {
        using (var scope = rootProvider.CreateScope())
        {
            var provider = scope.ServiceProvider;

            var logger1 = provider.GetRequiredService<ILoggerService>();
            var logger2 = provider.GetRequiredService<ILoggerService>();

            var orderCtx1 = provider.GetRequiredService<IOrderContextService>();
            var orderCtx2 = provider.GetRequiredService<IOrderContextService>();

            var shipping1 = provider.GetRequiredService<IShippingCalculator>();
            var shipping2 = provider.GetRequiredService<IShippingCalculator>();

            Console.WriteLine($"--- Request #{requestNumber} ---");
            Console.WriteLine($"Singleton cùng instance trong request? {logger1.InstanceId == logger2.InstanceId}");
            Console.WriteLine($"Scoped cùng instance trong request?    {orderCtx1.InstanceId == orderCtx2.InstanceId}");
            Console.WriteLine($"Transient cùng instance trong request? {shipping1.InstanceId == shipping2.InstanceId}");
        }
    }

    public static void Main()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ILoggerService, LoggerService>();
        services.AddScoped<IOrderContextService, OrderContextService>();
        services.AddTransient<IShippingCalculator, ShippingCalculator>();

        var rootProvider = services.BuildServiceProvider();

        SimulateHttpRequest(rootProvider, 1);
        SimulateHttpRequest(rootProvider, 2);
        // Output: --- Request #1 ---
        // Output: Singleton cùng instance trong request? True
        // Output: Scoped cùng instance trong request?    True
        // Output: Transient cùng instance trong request? False
        // Output: --- Request #2 ---
        // Output: Singleton cùng instance trong request? True
        // Output: Scoped cùng instance trong request?    True
        // Output: Transient cùng instance trong request? False
        // (Lưu ý: InstanceId của Singleton giữa Request #1 và #2 GIỐNG NHAU;
        //  InstanceId của Scoped giữa hai Request KHÁC NHAU — vì mỗi request là 1 scope mới)
    }
}
```

**Bẫy nguy hiểm nhất trong thực chiến:** inject một service `Scoped` (ví dụ `DbContext`) vào trong một service `Singleton` sẽ gây ra lỗi runtime **"Captive Dependency"** — service `Singleton` chỉ được tạo một lần khi app khởi động, nên nó sẽ "bắt giữ" mãi mãi instance `Scoped` đầu tiên đó, dùng xuyên suốt cho mọi request sau này (dữ liệu cũ, kết nối database bị đóng...). .NET Core mặc định sẽ ném exception ngay khi phát hiện việc này lúc build `ServiceProvider` với validation bật lên.

### 4.2. Cấu trúc cơ bản của một Controller API

```csharp
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")] // -> api/orders
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    // Constructor Injection: DI Container tự động "tiêm" dependency vào đây
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{id}")] // GET api/orders/5
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
            return NotFound(); // trả về HTTP 404

        return Ok(order); // trả về HTTP 200 + body JSON
    }

    [HttpPost] // POST api/orders
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState); // HTTP 400 nếu validation attribute fail

        var createdOrder = await _orderService.CreateAsync(request);

        // HTTP 201 + header Location trỏ tới resource vừa tạo — chuẩn RESTful
        return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
    }
}
```

**So với Spring Boot:** cấu trúc gần như song song 1-1 — `[ApiController]` ~ `@RestController`, `[Route]` ~ `@RequestMapping`, `[HttpGet]/[HttpPost]` ~ `@GetMapping/@PostMapping`, `[FromBody]` ~ `@RequestBody`. Điểm khác biệt lớn nhất về triết lý: Spring Boot dùng annotation + reflection khá "ma thuật" (magic), còn ASP.NET Core minh bạch hơn về pipeline middleware và convention rõ ràng qua `Program.cs`.

---

### GÓC PHỎNG VẤN — PHẦN 4

**1. Điều gì xảy ra nếu bạn inject một `Scoped` service vào một `Singleton` service? Tại sao nguy hiểm?**
> Trả lời sắc bén: Đây là lỗi "Captive Dependency". Vì Singleton chỉ khởi tạo một lần lúc ứng dụng start, dependency Scoped mà nó giữ cũng chỉ được tạo một lần duy nhất tại thời điểm đó rồi bị "giam giữ" mãi mãi — phá vỡ đúng mục đích thiết kế của Scoped (mỗi request một instance riêng, ví dụ DbContext với connection và tracking riêng). Hậu quả: rò rỉ bộ nhớ, dữ liệu cũ/stale, hoặc lỗi khi DbContext đã bị dispose nhưng vẫn bị dùng lại. .NET Core mặc định phát hiện và ném `InvalidOperationException` khi bật `ValidateScopes`.

**2. Constructor Injection và Property Injection khác nhau thế nào? Vì sao .NET Core khuyến nghị Constructor Injection?**
> Trả lời sắc bén: Constructor Injection buộc dependency phải được cung cấp **ngay khi object được tạo** — đảm bảo object luôn ở trạng thái hợp lệ (không bao giờ có `null` dependency), và giúp dependency là `readonly`/immutable. Property Injection cho phép gán sau, linh hoạt hơn nhưng dễ dẫn tới trạng thái object không đầy đủ giữa chừng. .NET Core DI container mặc định chỉ hỗ trợ tốt Constructor Injection — đây cũng là industry best-practice chung, không riêng .NET.

**3. `IActionResult` khác gì so với việc trả thẳng kiểu dữ liệu cụ thể (ví dụ trả thẳng `Order`) từ action method?**
> Trả lời sắc bén: `IActionResult` cho phép action linh hoạt trả về **nhiều loại HTTP response khác nhau** tùy tình huống (`Ok()`, `NotFound()`, `BadRequest()`, `CreatedAtAction()`...) — cần thiết khi logic có nhiều nhánh kết quả. Trả thẳng kiểu cụ thể (`Order`) chỉ phù hợp khi action luôn trả về HTTP 200 với đúng kiểu đó, đơn giản hơn nhưng kém linh hoạt khi cần xử lý lỗi/not-found tường minh. ASP.NET Core còn hỗ trợ `ActionResult<T>` — kết hợp cả hai, vừa type-safe cho Swagger/OpenAPI vừa linh hoạt trả các status code khác.

---

## PHẦN 5: BÀI TEST TƯ DUY TỔNG HỢP (MOCK INTERVIEW)

Ba tình huống dưới đây được trả lời theo chuẩn **STAR** (Situation – Task – Action – Result), mô phỏng cách một Senior trả lời trong buổi phỏng vấn thiết kế thực tế.

### Bài toán 1: Xử lý tồn kho khi nhiều đơn hàng đặt cùng một sản phẩm cùng lúc (Race Condition)

**Situation:** Hệ thống E-commerce có 1 sản phẩm còn 1 đơn vị tồn kho. Hai người dùng bấm "Đặt hàng" gần như cùng lúc.

**Task:** Đảm bảo không bao giờ bán vượt quá số lượng tồn kho thực tế (over-selling), dù có bao nhiêu request đồng thời.

**Action:** Thay vì đọc số lượng tồn kho rồi mới trừ ở tầng application (dễ dính race condition vì 2 request đều đọc thấy tồn kho = 1 trước khi request nào kịp ghi), giải pháp đúng là đẩy điều kiện kiểm tra xuống tận **câu lệnh UPDATE ở database**, dùng optimistic concurrency:

```csharp
public async Task<bool> TryReserveStockAsync(int productId, int quantity)
{
    // Điều kiện WHERE đảm bảo chỉ trừ kho khi tồn kho ĐỦ, thực hiện atomic tại DB
    int affectedRows = await _dbContext.Database.ExecuteSqlInterpolatedAsync(
        $"UPDATE Products SET Stock = Stock - {quantity} " +
        $"WHERE ProductId = {productId} AND Stock >= {quantity}");

    // affectedRows = 0 nghĩa là điều kiện WHERE không thỏa -> hết hàng, transaction khác đã "thắng"
    return affectedRows > 0;
}
```

**Result:** Với thiết kế này, dù 100 request bắn cùng lúc vào 1 sản phẩm còn tồn kho = 1, database engine tự đảm bảo tuần tự hóa (serialize) các câu UPDATE trên cùng dòng dữ liệu — chỉ đúng 1 request có `affectedRows > 0`, tất cả request còn lại nhận về `false` và hệ thống trả lỗi "Hết hàng" một cách chính xác, không cần lock thủ công ở tầng ứng dụng.

---

### Bài toán 2: Thiết kế luồng thanh toán tránh gọi trùng lặp (Idempotency)

**Situation:** Người dùng bấm nút "Thanh toán", do mạng chậm họ bấm thêm lần nữa trước khi có phản hồi. Cổng thanh toán bên thứ ba nhận được 2 request giống hệt nhau.

**Task:** Đảm bảo tiền chỉ bị trừ **đúng một lần** dù client gửi request trùng lặp.

**Action:** Áp dụng **Idempotency Key** — client sinh ra một GUID duy nhất cho mỗi ý định thanh toán (không phải mỗi lần bấm nút), gửi kèm trong header. Server lưu lại kết quả xử lý gắn với key đó:

```csharp
public async Task<PaymentResult> ProcessPaymentAsync(string idempotencyKey, decimal amount)
{
    // Kiểm tra key đã xử lý trước đó chưa (có thể lưu trong Redis/Database)
    var existingResult = await _idempotencyStore.GetResultAsync(idempotencyKey);
    if (existingResult != null)
    {
        return existingResult; // trả lại kết quả CŨ, không xử lý lại, không trừ tiền lần 2
    }

    var result = await _paymentGateway.ChargeAsync(amount);
    await _idempotencyStore.SaveResultAsync(idempotencyKey, result);
    return result;
}
```

**Result:** Client bấm bao nhiêu lần với cùng một `idempotencyKey` (ví dụ được sinh ra ngay khi trang thanh toán load lên) thì server vẫn chỉ thực thi giao dịch tài chính thật sự đúng 1 lần, các lần sau chỉ trả lại kết quả đã cache — loại bỏ hoàn toàn rủi ro double-charge cho khách hàng.

---

### Bài toán 3: Tối ưu truy vấn N+1 khi hiển thị danh sách đơn hàng kèm thông tin khách hàng

**Situation:** Trang quản trị hiển thị 100 đơn hàng, mỗi đơn hàng cần hiển thị kèm tên khách hàng. Code hiện tại gọi database 1 lần lấy danh sách đơn hàng, sau đó **lặp qua từng đơn** để gọi thêm 1 truy vấn lấy tên khách hàng tương ứng — tổng cộng 101 lượt truy vấn database.

**Task:** Giảm số lượng truy vấn xuống mức tối thiểu để tránh N+1 Query Problem, đảm bảo trang tải nhanh dù danh sách đơn hàng lớn.

**Action:** Dùng **Eager Loading** của Entity Framework Core (`Include`) để gộp truy vấn khách hàng ngay trong 1 câu SQL JOIN duy nhất, thay vì để code lặp và gọi rời rạc:

```csharp
// SAI: N+1 query — 1 query lấy Orders, sau đó N query lấy Customer cho từng Order
var orders = await _dbContext.Orders.ToListAsync();
foreach (var order in orders)
{
    order.Customer = await _dbContext.Customers.FindAsync(order.CustomerId); // N lần gọi DB!
}

// ĐÚNG: Eager Loading — chỉ 1 câu SQL với JOIN
var ordersWithCustomer = await _dbContext.Orders
    .Include(o => o.Customer)
    .AsNoTracking() // không cần theo dõi thay đổi vì chỉ đọc để hiển thị -> nhẹ hơn
    .ToListAsync();
```

**Result:** Từ 101 lượt gọi database giảm còn đúng 1 lượt, độ trễ trang quản trị giảm từ hàng giây (khi database ở xa, mỗi round-trip tốn 20-50ms) xuống chỉ còn vài chục miligiây. Đây cũng là lý do một Senior luôn bật log SQL (ví dụ qua `ILogger` của EF Core) trong môi trường dev để phát hiện sớm N+1 trước khi đưa lên production.

---

**Kết thúc cẩm nang.** Bạn đã đi qua đủ 5 phần: nền tảng OOP kiểu .NET, vũ khí Collections/Delegate/LINQ, bất đồng bộ async/await, Dependency Injection & Web API, và tư duy thiết kế hệ thống thực chiến qua 3 bài toán kinh điển. Nắm chắc phần này, bạn đủ tự tin bước vào bất kỳ vòng phỏng vấn Fresher/Junior .NET Backend nào.
