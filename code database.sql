CREATE DATABASE QUANLY_CUAHANG_BANXEMAY ON PRIMARY (
	NAME = 'QUANLY_CUAHANG_BANXEMAY',
	FILENAME = 'F:\QUANLY_CUAHANG_BANXEMAY.MDF',
	SIZE = 10MB,
	MAXSIZE = 50MB,
	FILEGROWTH = 10%
)
GO

USE QUANLY_CUAHANG_BANXEMAY
GO

-- Tài khoản đăng nhập
CREATE TABLE TAIKHOAN (
	TAIKHOAN NVARCHAR(50),
	MATKHAU NVARCHAR(50),
	QUYENHAN NVARCHAR(50)
);

-- Nhân viên
CREATE TABLE NHANVIEN (
	MANV VARCHAR(10) PRIMARY KEY,
	TENNV NVARCHAR(50),
	CHUCVU NVARCHAR(50),
	EMAIL NVARCHAR(50),
	SDTNV VARCHAR(10)
);

-- Khách hàng
CREATE TABLE KHACHHANG (
	MAKH VARCHAR(10) PRIMARY KEY,
	TENKH NVARCHAR(50),
	SDTKH VARCHAR(10),
	SOTIENCHI INT
);

-- Nhà cung cấp
CREATE TABLE NHACUNGCAP (
	MANCC VARCHAR(10) PRIMARY KEY,
	TENNCC NVARCHAR(50),
	DIACHI NVARCHAR(255),
	SDTNCC VARCHAR(10)
);

-- Xe máy
CREATE TABLE XEMAY (
	MAXE VARCHAR(10) PRIMARY KEY,
	TENXE NVARCHAR(50),
	HANGSX NVARCHAR(50),
	NAMSX VARCHAR(10),
	TINHTRANG NVARCHAR(50),
	NGUONGOC NVARCHAR(50),
	ANH image,
	SOLUONG INT,
	GIABAN INT
);




-- Nhập hàng
CREATE TABLE NHAPHANG (
	MANHAP VARCHAR(10) PRIMARY KEY,
	NGAYNHAP DATE,
	TONGTIEN INT,
	MANV VARCHAR(10),
	MANCC VARCHAR(10),
	FOREIGN KEY (MANV) REFERENCES NHANVIEN(MANV),
	FOREIGN KEY (MANCC) REFERENCES NHACUNGCAP(MANCC)
);

-- Chi tiết nhập hàng
CREATE TABLE CT_NHAPHANG (
	MANHAP VARCHAR(10),
	MAXE VARCHAR(10),
	SOLUONG INT,
	DONGIA INT,
	PRIMARY KEY (MANHAP, MAXE),
	FOREIGN KEY (MANHAP) REFERENCES NHAPHANG(MANHAP),
	FOREIGN KEY (MAXE) REFERENCES XEMAY(MAXE)
);

-- Bảo hiểm
CREATE TABLE BAOHIEM (
	MABH VARCHAR(10),
	MAXE VARCHAR(10),
	THOIHAN int,
	PRIMARY KEY (MABH, MAXE),
	FOREIGN KEY (MAXE) REFERENCES XEMAY(MAXE)
);

-- Hóa đơn
CREATE TABLE HOADON (
	MAHD VARCHAR(10) PRIMARY KEY,
	NGAYLAP DATE,
	TONGTIEN INT,
	MAKH VARCHAR(10),
	MANV VARCHAR(10),
	FOREIGN KEY (MAKH) REFERENCES KHACHHANG(MAKH),
	FOREIGN KEY (MANV) REFERENCES NHANVIEN(MANV)
);

-- Chi tiết hóa đơn
CREATE TABLE CT_HOADON (
	MAHD VARCHAR(10),
	MAXE VARCHAR(10),
	SOLUONG INT,
	DONGIA INT,
	PRIMARY KEY (MAHD, MAXE),
	FOREIGN KEY (MAXE) REFERENCES XEMAY(MAXE),
	FOREIGN KEY (MAHD) REFERENCES HOADON(MAHD)
);

-- Thủ tục đăng nhập
CREATE PROCEDURE dangnhap (
	@ten NVARCHAR(50),
	@matkhau NVARCHAR(50)
)
AS
BEGIN
	SELECT t.QUYENHAN
	FROM TAIKHOAN t
	WHERE t.TAIKHOAN = @ten AND t.MATKHAU = @matkhau
END;


--  thong tin nha cung cap

	CREATE PROCEDURE themncc (
	@mancc VARCHAR(10),
	@tenncc NVARCHAR(50),
	@diachi NVARCHAR(255),
	@sdtncc VARCHAR(10)
)
AS
BEGIN
	INSERT INTO NHACUNGCAP (MANCC, TENNCC, DIACHI, SDTNCC)
	VALUES (@mancc, @tenncc, @diachi, @sdtncc);
END;


CREATE PROCEDURE xoanhacc (
    @mancc VARCHAR(10)
)
AS
BEGIN
    DELETE FROM NHACUNGCAP
    WHERE MANCC = @mancc;
END;


CREATE PROCEDURE suancc_dongian (
    @mancc VARCHAR(10),
    @tenncc NVARCHAR(50) = NULL,
    @diachi NVARCHAR(255) = NULL,
    @sdtncc VARCHAR(10) = NULL
)
AS
BEGIN
    UPDATE NHACUNGCAP
    SET
        TENNCC = COALESCE(@tenncc, TENNCC),
        DIACHI = COALESCE(@diachi, DIACHI),
        SDTNCC = COALESCE(@sdtncc, SDTNCC)
    WHERE MANCC = @mancc;
END;

CREATE PROCEDURE timkiemncc
    @mancc VARCHAR(10) = NULL,
    @tenncc NVARCHAR(50) = NULL
AS
BEGIN
    SELECT MANCC N'Mã Nhà cung cấp', TENNCC N'Họ tên', DIACHI N'Địa chỉ',SDTNCC N'Số điện thoại' FROM NHACUNGCAP
    WHERE
        (@mancc IS NULL OR MANCC LIKE '%' + @mancc + '%') AND
        (@tenncc IS NULL OR TENNCC LIKE N'%' + @tenncc + '%')
END

-- TAIKHOAN
INSERT INTO TAIKHOAN VALUES
(N'admin', N'admin123', N'Admin'),
(N'nhanvien01', N'nv123', N'NhanVien'),
(N'khach01', N'khach123', N'KhachHang');

-- NHANVIEN
INSERT INTO NHANVIEN VALUES
('NV001', N'Nguyễn Văn A', N'Quản lý', 'a.nguyen@example.com', '0901234567'),
('NV002', N'Trần Thị B', N'Nhân viên bán hàng', 'b.tran@example.com', '0912345678');

-- KHACHHANG
INSERT INTO KHACHHANG VALUES
('KH001', N'Lê Văn C', '0923456789', 50000000),
('KH002', N'Phạm Thị D', '0934567890', 75000000);

-- NHACUNGCAP
INSERT INTO NHACUNGCAP VALUES
('NCC001', N'Công ty Yamaha', N'123 Lý Thường Kiệt, Q.10, TP.HCM', '0945678901'),
('NCC002', N'Công ty Honda', N'456 Nguyễn Trãi, Q.5, TP.HCM', '0956789012');

-- XEMAY
INSERT INTO XEMAY (MAXE, TENXE, HANGSX, NAMSX, TINHTRANG, NGUONGOC, ANH) VALUES
('XE001', N'Exciter 150', N'Yamaha', '2022', N'Mới', N'Việt Nam', NULL),
('XE002', N'Winner X', N'Honda', '2023', N'Mới', N'Nhật Bản', NULL);

-- NHAPHANG
INSERT INTO NHAPHANG VALUES
('NH001', '2025-06-01', 60000000, 'NV001', 'NCC001'),
('NH002', '2025-06-03', 70000000, 'NV002', 'NCC002');

-- CT_NHAPHANG
INSERT INTO CT_NHAPHANG VALUES
('NH001', 'XE001', 3, 20000000),
('NH002', 'XE002', 2, 35000000);

-- BAOHIEM
INSERT INTO BAOHIEM VALUES
('BH001', 'XE001', 1),
('BH002', 'XE002', 2 );

-- HOADON
INSERT INTO HOADON VALUES
('HD001', '2025-06-05', 40000000, 'KH001', 'NV001'),
('HD002', '2025-06-06', 35000000, 'KH002', 'NV002');

-- CT_HOADON
INSERT INTO CT_HOADON VALUES
('HD001', 'XE001', 2, 20000000),
('HD002', 'XE002', 1, 35000000);


CREATE PROCEDURE LAYHOADONNHAP
AS 
BEGIN

	SELECT NH.MANHAP N'Mã hóa đơn', NH.MANCC N'Mã nhà cung cấp', NH.MANV N'Mã nhân viên',NH.TONGTIEN N'Tổng tiền',NH.NGAYNHAP N'Ngày lập'
	FROM NHAPHANG NH
END
drop PROCEDURE LAYHOADONNHAP

Create procedure laymahoadonnhap
as
begin
	SELECT NH.MANHAP N'Mã hóa đơn'
	FROM NHAPHANG NH
end

create procedure laychitiethoadonnhap(
@mahd char(10))as
begin
	select ctnh.MANHAP N'Mã hóa đơn',ctnh.MAXE  N'Mã Xe', ctnh.SOLUONG N'Số lượng' , ctnh.DONGIA N'Đơn giá'
	from CT_NHAPHANG ctnh
	where trim(ctnh.MANHAP)=trim(@mahd)

end

laychitiethoadonnhap 'NH001'

create procedure timkiemhoadonnhap(
@ngay date
)as

begin

	SELECT NH.MANHAP N'Mã hóa đơn', NH.MANCC N'Mã nhà cung cấp', NH.MANV N'Mã nhân viên',NH.TONGTIEN N'Tổng tiền',NH.NGAYNHAP N'Ngày lập'
	FROM NHAPHANG NH
	where NH.NGAYNHAP=@ngay
end


CREATE PROCEDURE LAYHOADONBAN
AS 
BEGIN

	SELECT NH.MAHD N'Mã hóa đơn', NH.MAKH N'Mã khách hàng ',NH.MANV N'Mã nhân viên',NH.TONGTIEN N'Tổng tiền',NH.NGAYLAP N'Ngày lập'
	FROM HOADON NH
END

CREATE PROCEDURE LAYmaHOADONBAN
AS 
BEGIN

	SELECT NH.MAHD N'Mã hóa đơn'
	FROM HOADON NH
END

create procedure laychitiethoadonban(
@mahd char(10))as
begin
	select ctnh.MAHD N'Mã hóa đơn',ctnh.MAXE  N'Mã Xe', ctnh.SOLUONG N'Số lượng' , ctnh.DONGIA N'Đơn giá'
	from CT_HOADON ctnh
	where trim(ctnh.MAHD)=trim(@mahd)

end

create procedure timkiemhoadonban(
@ngay date
)as

begin

	SELECT NH.MAHD N'Mã hóa đơn', NH.MAKH N'Mã khách hàng ',NH.MANV N'Mã nhân viên',NH.TONGTIEN N'Tổng tiền',NH.NGAYLAP N'Ngày lập'
	FROM HOADON NH
	where NH.NGAYLAP=@ngay
end

drop procedure laymaxemay 
create procedure laymaxemay 
as begin
	select xm.MAXE N'Mã xe'
	from XEMAY xm
	where xm.MAXE !='XE000'
end

create procedure timkiemmaxemay(@maxe char(10))
as begin
	select xm.MAXE N'Mã xe'
	from XEMAY xm
	where TRIM(xm.MAXE)=trim(@maxe)
end


create procedure timkiemtenxemay(@maxe char(10))
as begin
	select xm.TENXE N'Tên xe'
	from XEMAY xm
	where TRIM(xm.MAXE)=trim(@maxe)
end

create procedure laydanhsachbaohanh as begin
	select bh.MABH N'Mã bảo hành' ,bh.MAXE N'Mã Xe', bh.THOIHAN N'Thời hạn'
	from BAOHIEM bh

end

create procedure timkiemmabaonhanh (@mabh char(10))as 
begin
select bh.MABH N'Mã bảo hành' 
	from BAOHIEM bh
	where trim(bh.MABH)=trim(@mabh)
end
create procedure checkxecobaohanhchua (@maxe char(10))as
begin
select bh.MAXE N'Mã xe' 
	from BAOHIEM bh
	where trim(bh.MAXE)=trim(@maxe)
end
create procedure thembaohanh(
@mabh char(10),
@maxe char (10),
@thoihan int
)as
begin
insert into BAOHIEM (MABH,MAXE,THOIHAN) values (@mabh,@maxe,@thoihan)
end

thembaohanh  'BH003','XE003',1

create procedure xoabaohanh(@mabh char(10))as 
begin
	delete from BAOHIEM
	where trim(MABH)=trim(@mabh)
end 
create procedure suabaohanh (
@mabh char(10),
@thoihan int
)
as begin
	update BAOHIEM
	set THOIHAN=@thoihan
	where trim(MABH)=trim(@mabh)
end


CREATE PROCEDURE checkhoadonconbaohanhkhong
(
    @hoadon CHAR(10),
    @date DATE
)
AS
BEGIN
	select xm.MAXE N'Mã xe', xm.TENXE N'Tên xe'
	from XEMAY xm
	where xm.MAXE in( SELECT bh.MAXE 
    FROM BAOHIEM bh
    JOIN CT_HOADON cthd ON bh.MAXE = cthd.MAXE
    JOIN HOADON hd ON hd.MAHD = cthd.MAHD
    WHERE hd.MAHD = @hoadon
      AND DATEADD(YEAR, bh.THOIHAN, hd.NGAYLAP) > @date)
END
drop PROCEDURE checkhoadonconbaohanhkhong

create procedure timkiemhoadon (@mahd char(10))
as 
begin
	select hd.MAHD N'Mã hóa đơn'
	from HOADON hd
	where trim(hd.MAHD)=trim(@mahd)
end



create procedure laydanhsachkhachhang
as

begin


	select kh.MAKH N'Mã khách hàng',kh.TENKH N'Tên khách hàng',kh.SDTKH N'Số điện thoại', kh.SOTIENCHI N'Số tiền chi'
	from KHACHHANG kh

end

create procedure timkiemmakhachhang(@makh char(10))
as begin
select kh.MAKH N'Mã khách hàng'
	from KHACHHANG kh
	where trim(kh.MAKH)=trim(@makh)
end
CREATE PROCEDURE timkiemsdtkhachhang
    @makh CHAR(10),
    @sdt VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT kh.SDTKH AS N'Số điện thoại'
    FROM KHACHHANG kh
    WHERE LTRIM(RTRIM(kh.MAKH)) != LTRIM(RTRIM(@makh))
      AND LTRIM(RTRIM(kh.SDTKH)) = LTRIM(RTRIM(@sdt))
END
create procedure themkhachhang(@makh char(10),@tenkh varchar(50),@sdt varchar(10),@sotienchi int)
as 
begin

	insert into KHACHHANG (MAKH,TENKH,SDTKH,SOTIENCHI) values (@makh,@tenkh,@sdt,@sotienchi)

end

create procedure suakhachhang(@makh char(10),@tenkh varchar(50),@sdt varchar(10),@sotienchi int)
as 
begin

 update KHACHHANG
 set TENKH=@tenkh,SDTKH=@sdt,SOTIENCHI=@sotienchi
 where trim(MAKH)=trim(@makh)
end


create procedure xoakhachhang(@makh char (10))as
begin
	update HOADON
	set MAKH='KH000'
	where trim(@makh)=trim(MAKH)
	delete from KHACHHANG
	where trim(@makh)=trim(MAKH)
end
  drop PROCEDURE laydanhsachtimkiemkhachhang 
  laydanhsachtimkiemkhachhang  '001','',''
CREATE PROCEDURE laydanhsachtimkiemkhachhang (
    @makh CHAR(10) = NULL,
    @tenkh NVARCHAR(50) = NULL,
    @sdt VARCHAR(10) = NULL
)
AS
BEGIN
    SELECT 
        kh.MAKH AS N'Mã khách hàng',
        kh.TENKH AS N'Tên khách hàng',
        kh.SDTKH AS N'Số điện thoại',
        kh.SOTIENCHI AS N'Số tiền chi'
    FROM KHACHHANG kh
    WHERE (@makh IS NULL OR LTRIM(RTRIM(kh.MAKH)) LIKE '%' + LTRIM(RTRIM(@makh)) + '%')
      AND (@tenkh IS NULL OR LTRIM(RTRIM(kh.TENKH)) LIKE '%' + LTRIM(RTRIM(@tenkh)) + '%')
      AND (@sdt IS NULL OR LTRIM(RTRIM(kh.SDTKH)) LIKE '%' + LTRIM(RTRIM(@sdt)) + '%')
END

CREATE PROCEDURE laymanhaphanglonnhat
AS
BEGIN
    SELECT TOP 1 nh.MANHAP
    FROM NHAPHANG nh
    ORDER BY nh.MANHAP DESC  -- Nếu muốn mã lớn nhất (nghĩa là mã mới nhất)
END 
create procedure laytencacnhacungcap
as
begin
	select TENNCC 
	from NHACUNGCAP
end

CREATE PROCEDURE laymaxelonnhat
AS
BEGIN
    SELECT TOP 1 xm.MAXE
    FROM XEMAY xm
    ORDER BY xm.MAXE DESC  
END 

create procedure themxe(
@maxe varchar(10),
@tenxe nvarchar(50),
@hangsx nvarchar(50),
@namsx varchar(10),
@tinhtrang nvarchar(50),
@nguongoc nvarchar(50),
@anh image,
@soluong int
)
as
begin
insert into XEMAY(MAXE,TENXE,HANGSX,NAMSX,TINHTRANG,NGUONGOC,ANH,SOLUONG,GIABAN) values (@maxe,@tenxe,@hangsx,@namsx,@tinhtrang,@nguongoc,@anh,@soluong,0)
end

create procedure taohoadonnhap(
@mahd varchar(10),
@ngaylap date,
@tongtien int,
@manv varchar(10),
@mancc varchar(10)
)
as begin
	insert into NHAPHANG(MANHAP,NGAYNHAP,TONGTIEN,MANV,MANCC) values (@mahd,@ngaylap,@tongtien,@manv,@mancc)
end

CREATE PROCEDURE taochitietnhap
    @manhap VARCHAR(10),
    @maxe VARCHAR(10),
    @soluong INT,
    @dongia INT
AS
BEGIN
    INSERT INTO CT_NHAPHANG (MANHAP, MAXE, SOLUONG, DONGIA)
    VALUES (LTRIM(RTRIM(@manhap)), LTRIM(RTRIM(@maxe)), @soluong, @dongia);
END

drop procedure taochitietnhap

create procedure laymanhacungcap(
@ten nvarchar(50))as
begin
	select ncc.MANCC
	from NHACUNGCAP ncc
	where trim(ncc.TENNCC)=trim(@ten)
end
create procedure laymanhanvientutaikhoan(@tendangnhap nvarchar(50))
as
begin
	select tk.Manv
	from  TAIKHOAN tk
	where tk.TAIKHOAN =@tendangnhap
end
drop procedure layxemay
create procedure layxemay
as
begin
	select xm.MAXE N'Mã xe',xm.TENXE N'Tên xe',xm.HANGSX N'Hãng sản xuất',xm.NAMSX N'Năm sản xuất',xm.TINHTRANG N'Tình trạng', xm.NGUONGOC N'Nguồn gốc', xm.SOLUONG N'Số lượng',xm.GIABAN N'Giá bán',xm.ANH
	from XEMAY xm
	where trim(xm.MAXE)!='XE000'
end

create procedure laythongtinxeban(
@maxe varchar(10)
)as
begin
	select xm.TENXE ,xm.HANGSX,xm.NAMSX,xm.TINHTRANG,xm.NGUONGOC,xm.SOLUONG,xm.GIABAN 
	from XEMAY xm
	where  trim(xm.MAXE)=trim(@maxe)
end


create procedure laymakhtusdt(@sdt varchar(10))
as begin
	select kh.MAKH
	from KHACHHANG kh
	where trim(kh.SDTKH)=trim(@sdt)
end
create procedure laymahdlonnhat
as 
begin
	select top 1 MAHD
	from HOADON
	order by MAHD desc
end


create procedure taohoadonban(
@mahd varchar(10),
@ngay date,
@tongtien int,
@makh varchar(10),
@manv varchar(10)
)as
begin
	UPDATE KHACHHANG
SET SOTIENCHI = ISNULL(SOTIENCHI, 0) + @tongtien
WHERE TRIM(MAKH) = TRIM(@makh)
insert into HOADON(MAHD,NGAYLAP,TONGTIEN,MAKH,MANV) values (@mahd,@ngay,@tongtien,@makh,@manv)
end
create procedure taoctban(
@mahd varchar(10),
@maxe varchar(10),
@soluong int,
@dongia int
)
as
begin
	update XEMAY
	set SOLUONG=SOLUONG-@soluong
	where trim(MAXE)=trim(@maxe)
	insert into CT_HOADON(MAHD,MAXE,SOLUONG,DONGIA) values (@mahd,@maxe,@soluong,@dongia)


end

CREATE PROCEDURE suaxemay
(
    @maxe VARCHAR(10),
    @tenxe NVARCHAR(50),
    @hangxe NVARCHAR(50),
    @namsx VARCHAR(10),
    @tinhtrang nVARCHAR(10),
    @nguongoc VARCHAR(50),
    @giaxe INT,
    @anh IMAGE
)
AS
BEGIN
    -- Cập nhật thông tin trong bảng XEMAY nếu mã xe tồn tại
    UPDATE XEMAY
    SET 
        TENXE     = @tenxe,
        HANGSX    = @hangxe,
        NAMSX     = @namsx,
        TINHTRANG = @tinhtrang,
        NGUONGOC  = @nguongoc,
        GIABAN    = @giaxe,
        ANH       = @anh
    WHERE LTRIM(RTRIM(MAXE)) = LTRIM(RTRIM(@maxe));
END
