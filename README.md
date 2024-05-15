
<div align="center">
<h1>
    <img alt="MARKDOWN SYNTAX" src="https://github.com/hellOoSaksit/Moondust-Websit/assets/79570387/f806c19a-13f3-40f7-8222-e8fe07c94718"></img>
</h1>
</div>

<h1 align="center">Project Moondust</h1>
<h3 align="center">อยากทำไปเรื่อยๆ</h3></p>

| id | name | description | status ❌⭕️ |            
|----|------|-------|----------|
| 1  | Login | - | ⭕️ |
| 2  | Register | - | ⭕️ |
| 3  | EditProdect | - | ⭕️ |
| 4  | AddProdect | - | ⭕️ |
| 5  | DeleteProdut | - | ⭕️ |
| 6  | AddMember | - | ⭕️ |
| 7  | EditMember | - | ⭕️ |
| 8  | DeleteMember | - | ⭕️ |
| 9  | DeleteMember | - | ⭕️ |
| 10  | AddCategory | - | ⭕️ |
| 11  | EditCategory | - | ⭕️ |
| 12  | DeleteCategory | - | ⭕️ |
| 13  | DeleteMember | - | ⭕️ |
| 14  | bills | - | ⭕️ |
<br><br>

<br><br>
<h1>วิธีติดตั้ง</h1>



<h2>
DATABASE NAME : MoonDust </br>
</h2>
-SQL ที่ใช้งาน : PostgreSQL </br>
1.สร้าง SQL ชื่อ Moondust</br>
2.รันไฟล์ SQL</br>


```

CREATE TABLE IF NOT EXISTS "avater_images" (
	"image_id" SERIAL NOT NULL,
	"filename" VARCHAR(255) NOT NULL,
	"content_type" VARCHAR(255) NOT NULL,
	"data" BYTEA NOT NULL,
	"uploaded_at" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"owner_username" VARCHAR(60) NOT NULL,
	PRIMARY KEY ("image_id"),
	UNIQUE "unique_owner_username" ("owner_username"),
	CONSTRAINT "images_owner_username_fkey" FOREIGN KEY ("owner_username") REFERENCES "users" ("username") ON UPDATE NO ACTION ON DELETE NO ACTION
);

CREATE TABLE IF NOT EXISTS "bills" (
	"id" SERIAL NOT NULL,
	"order_number" INTEGER NOT NULL,
	"itemdata" JSONB NOT NULL,
	"date" DATE NOT NULL,
	"time" TIME NOT NULL,
	PRIMARY KEY ("id")
);

INSERT INTO "bills" ("id", "order_number", "itemdata", "date", "time") VALUES
	(18, 1, '{"date": "5/13/2024 12:00:00 AM", "time": "16:05:00.2535686", "itemName": "อาหารทะเล", "itemQuantity": 100.00, "itemPriceTotal": 100.00, "itemPerUnitPrice": 1}', '2024-05-13', '16:05:00.253568'),
	(19, 1, '{"date": "5/13/2024 12:00:00 AM", "time": "16:05:00.2585850", "itemName": "ของหวาน", "itemQuantity": 30.00, "itemPriceTotal": 30.00, "itemPerUnitPrice": 1}', '2024-05-13', '16:05:00.258585');


CREATE TABLE IF NOT EXISTS "category" (
	"category_id" SERIAL NOT NULL,
	"category_name" VARCHAR(50) NOT NULL,
	PRIMARY KEY ("category_id")
);


INSERT INTO "category" ("category_id", "category_name") VALUES
	(1, 'Food'),
	(3, 'Dessert'),
	(2, 'Drinks'),
	(4, 'Smoothie'),
	(11, 'ทดสอบ 2 ');


-- Dumping structure for table public.product_item
CREATE TABLE IF NOT EXISTS "product_item" (
	"product_id" SERIAL NOT NULL,
	"product_name" VARCHAR(60) NOT NULL,
	"product_des" VARCHAR(100) NOT NULL,
	"product_category" VARCHAR(100) NOT NULL,
	"product_price" MONEY NOT NULL DEFAULT '$0.00',
	"product_link" TEXT NULL DEFAULT 'NoLink',
	PRIMARY KEY ("product_id"),
	UNIQUE "unique_product_name" ("product_name")
);


INSERT INTO "product_item" ("product_id", "product_name", "product_des", "product_category", "product_price", "product_link") VALUES
	(66, 'โกโก้', 'โกโก้', 'Dessert', $30.00, 'https://i.imgur.com/gBp5Piz.jpeg'),
	(67, 'ของหวาน', 'ของหวาน', 'Dessert', $30.00, 'https://i.imgur.com/cTgpWIM.jpeg'),
	(68, 'อาหารทะเล', 'อาหารทะเล', 'Food', $100.00, 'https://i.imgur.com/5aIcUqx.jpeg'),
	(65, 'ชาเขียว', 'เครื่องดืม ดีๆ', 'Drinks', $50.00, 'https://i.imgur.com/8yb0QST.jpeg');

CREATE TABLE IF NOT EXISTS "product_option" (
	"option_id" SERIAL NOT NULL,
	"product_name" VARCHAR(60) NOT NULL,
	"product_option" VARCHAR(50) NOT NULL,
	PRIMARY KEY ("option_id"),
	CONSTRAINT "FK_product_option_product_item" FOREIGN KEY ("product_name") REFERENCES "product_item" ("product_name") ON UPDATE NO ACTION ON DELETE NO ACTION
);

CREATE TABLE IF NOT EXISTS "users" (
	"user_id" SERIAL NOT NULL,
	"username" VARCHAR(60) NOT NULL,
	"password" VARCHAR(255) NOT NULL,
	"name" VARCHAR(60) NOT NULL,
	"email" VARCHAR(100) NOT NULL,
	"admin" BOOLEAN NOT NULL DEFAULT false,
	UNIQUE "users_email_key" ("email"),
	PRIMARY KEY ("user_id"),
	UNIQUE "users_username_key" ("username")
);

```

<div align="center">

<h3>ตัวอย่างโปรแกรม</h3>
 
![image](https://github.com/hellOoSaksit/POS_MoonDust/assets/79570387/3d7600db-8b96-4e18-94e8-76649d08c071)

![image](https://github.com/hellOoSaksit/POS_MoonDust/assets/79570387/a96e3933-b271-4cd0-8b21-ba0242544362)

![image](https://github.com/hellOoSaksit/POS_MoonDust/assets/79570387/29d2fcdc-dfaf-4acb-ba07-4e27eb9067eb)

![image](https://github.com/hellOoSaksit/POS_MoonDust/assets/79570387/f7589d58-12df-4325-b771-a9169a0b8f65)

![image](https://github.com/hellOoSaksit/POS_MoonDust/assets/79570387/b3895c8a-e8ab-4e6d-b115-32919f8fef96)

![image](https://github.com/hellOoSaksit/POS_MoonDust/assets/79570387/f8dfc079-3023-49eb-b72d-2c29e09e5b60)

![image](https://github.com/hellOoSaksit/POS_MoonDust/assets/79570387/d8f1e5a6-53f3-4004-a36a-827618862dd5)

</div>

