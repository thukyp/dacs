-- Xóa dữ liệu cũ nếu cần (CẨN THẬN KHI CHẠY LỆNH DELETE!)
-- DELETE FROM LoaiSanPhams;

-- === NGUYÊN LIỆU THÔ ===
INSERT INTO LoaiSanPhams (M_LoaiSP, TenLoai) VALUES
('NLT01', N'Rơm rạ'),
('NLT02', N'Vỏ trấu'),
('NLT03', N'Bã mía'),
('NLT04', N'Lõi ngô / Cùi bắp'),
('NLT05', N'Thân cây nông nghiệp'),
('NLT06', N'Vỏ cà phê / điều'),
('NLT07', N'Xơ dừa');

-- === ĐÃ QUA XỬ LÝ ===
INSERT INTO LoaiSanPhams (M_LoaiSP, TenLoai) VALUES
('DQX01', N'Nghiền / Xay'),
('DQX02', N'Ép khối / Băm nhỏ'),
('DQX03', N'Viên nén năng lượng (Đã xử lý)'), -- Phân biệt với NLSH
('DQX04', N'Phơi khô / Sấy'),
('DQX05', N'Ủ chua / Lên men');

-- === THỨC ĂN CHĂN NUÔI ===
INSERT INTO LoaiSanPhams (M_LoaiSP, TenLoai) VALUES
('TACN01', N'Thức ăn gia súc'),
('TACN02', N'Thức ăn gia cầm'),
('TACN03', N'Thức ăn thủy sản'),
('TACN04', N'Nguyên liệu TĂCN'),
('TACN05', N'Phụ gia TĂCN');

-- === PHÂN BÓN & GIÁ THỂ ===
INSERT INTO LoaiSanPhams (M_LoaiSP, TenLoai) VALUES
('PBGT01', N'Phân compost hữu cơ'),
('PBGT02', N'Phân trùn quế'),
('PBGT03', N'Giá thể trồng nấm'),
('PBGT04', N'Giá thể trồng cây'),
('PBGT05', N'Vi sinh cải tạo đất');

-- === NĂNG LƯỢNG SINH KHỐI ===
INSERT INTO LoaiSanPhams (M_LoaiSP, TenLoai) VALUES
('NLSH01', N'Viên nén (Trấu, Gỗ,...)'),
('NLSH02', N'Củi ép / Briket'),
('NLSH03', N'Dăm gỗ'),
('NLSH04', N'Khí sinh học (BIOGAS)'),
('NLSH05', N'Dầu sinh học');

-- === DỊCH VỤ ===
INSERT INTO LoaiSanPhams (M_LoaiSP, TenLoai) VALUES
('DV01', N'Thu gom tại nguồn'),
('DV02', N'Vận chuyển'),
('DV03', N'Xử lý theo yêu cầu'),
('DV04', N'Tư vấn giải pháp');

-- === THIẾT BỊ ===
INSERT INTO LoaiSanPhams (M_LoaiSP, TenLoai) VALUES
('TB01', N'Máy nghiền / Băm'),
('TB02', N'Máy ép viên nén'),
('TB03', N'Máy ép khối'),
('TB04', N'Máy sấy nông sản');

-- === VẬT TƯ KHÁC ===
INSERT INTO LoaiSanPhams (M_LoaiSP, TenLoai) VALUES
('VTK01', N'Bao bì đóng gói'),
('VTK02', N'Chế phẩm sinh học');

-- === ĐƠN VỊ TÍNH (VÍ DỤ) ===
-- Giả sử bạn cũng muốn thêm vào bảng DonViTinhs
INSERT INTO DonViTinhs (M_DonViTinh, TenLoaiTinh) VALUES
('KG', N'Kilogram'),
('Tan', N'Tấn'),
('Bao', N'Bao'),
('MetKhoi', N'Mét khối'),
('Chuyen', N'Chuyến'),
('Cai', N'Cái');


INSERT INTO KhoLuuTrus (M_KhoLuuTru, SoLuongTon, NgayTao, HanSuDung)
VALUES
('KHO01', 15000, GETUTCDATE(), '2025-12-31'), -- Kho chính số 1, tồn 15000, HSD cuối năm 2025
('KHO02', 7500, GETUTCDATE(), '2026-06-30'), -- Kho phụ số 2, tồn 7500, HSD giữa năm 2026
('KHO_A', 0, GETUTCDATE(), '2025-10-31'), -- Kho A, đã hết hàng (tồn 0), HSD tháng 10/2025
('KHO_B', 123456, GETUTCDATE(), '2027-01-15'), -- Kho B, tồn nhiều, HSD tháng 1/2027
('KHO_ROM', 20000, GETUTCDATE(), '2026-02-28'), -- Ví dụ Kho chuyên chứa Rơm, HSD tháng 2/2026
('KHO_TRAU', 50000, GETUTCDATE(), '2026-09-30'); -- Ví dụ Kho chuyên chứa Trấu, HSD tháng 9/2026


