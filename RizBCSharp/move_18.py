# 导入模块
import os
import shutil
import random

# 定义函数
def move_files(path):
    # 初始化重名文件计数器
    duplicate_count = 0
    # 遍历子目录和文件
    for root, dirs, files in os.walk(path):
        # 对于每一个文件
        for file in files:
            # 获取文件的完整路径
            src = os.path.join(root, file)
            # 获取文件的目标路径
            dst = os.path.join(path, file)
            # 检查是否有重名文件
            if os.path.exists(dst):
                # 增加重名文件计数器
                duplicate_count += 1
                # 在文件名后面加上一个随机数
                name, ext = os.path.splitext(file)
                name += f"_{random.randint(1, 100)}"
                file = name + ext
                dst = os.path.join(path, file)
            # 移动文件
            shutil.move(src, dst)
            # 打印信息
            print(f"Moved {src} to {dst}")
    # 返回重名文件计数器
    return duplicate_count

# 调用函数
duplicate_count = move_files(os.getcwd())
# 打印结果
print(f"There are {duplicate_count} duplicate files in total.")