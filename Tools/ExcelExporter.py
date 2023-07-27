import xlrd
import sys
import os

# TAP的空格数
from xlrd.timemachine import unicode

TAP_BLANK_NUM = 4

FIELD_RULE_ROW = 0

FIELD_NAME_ROW = 1
FIELD_TYPE_ROW = 2
FIELD_COMMENT_ROW = 3
FIELD_VALUE_ROW = 4

TEMP_DIR = "tmp"

class LogHelp:
    """日志辅助类"""
    _logger = None
    _close_imme = True

    @staticmethod
    def set_close_flag(flag):
        LogHelp._close_imme = flag

    @staticmethod
    def _initlog():
        import logging

        LogHelp._logger = logging.getLogger()
        if os.path.exists(TEMP_DIR):
            TEMP_DIR
        else:
            os.mkdir(TEMP_DIR)
        logfile = TEMP_DIR + '/tnt_comm_deploy_tool.log'
        hdlr = logging.FileHandler(logfile)
        formatter = logging.Formatter('%(asctime)s|%(levelname)s|%(lineno)d|%(funcName)s|%(message)s')
        hdlr.setFormatter(formatter)
        LogHelp._logger.addHandler(hdlr)
        LogHelp._logger.setLevel(logging.NOTSET)
        # LogHelp._logger.setLevel(logging.WARNING)

        LogHelp._logger.info("\n\n\n")
        LogHelp._logger.info("logger is inited!")

    @staticmethod
    def get_logger():
        if LogHelp._logger is None:
            LogHelp._initlog()

        return LogHelp._logger

    @staticmethod
    def close():
        if LogHelp._close_imme:
            import logging
            if LogHelp._logger is None:
                return
            logging.shutdown()

# log macro
LOG_DEBUG = LogHelp.get_logger().debug
LOG_INFO = LogHelp.get_logger().info
LOG_WARN = LogHelp.get_logger().warning
LOG_ERROR = LogHelp.get_logger().error

class SheetInterpreter:
    """通过excel配置生成配置文件"""
    def __init__(self, xls_path, txtName):
        # xls 的路径
        self._xls_file_path = xls_path
        # sheet 的名称
        self._txtName = txtName


        # 打开指定的excel
        try:
            self._workbook = xlrd.open_workbook(self._xls_file_path)
        except BaseException as e:
            print("open xls file(%s) failed!" % (self._xls_file_path))
            raise
        
        # 获取打开的Excel的第一个sheet
        try:
            self._sheet = self._workbook.sheet_by_index(0)
            self._sheetName = self._sheet.name
        except BaseException as e:
            print("open sheet(%s) failed!" % (self._sheetName))

        # 行数和列数
        self._row_count = len(self._sheet.col_values(0))
        self._col_count = len(self._sheet.row_values(0))
        self._row = 0
        self._col = 0

        # 将所有的输出先写到一个list， 最后统一写到文件
        self._output = []

        # 保存所有结构的名字
        self._struct_name_list = []

        #保存字段和导出数据
        self._item = []
        self._items = []


    def OutputDataBySheet(self):
        """对外的接口"""
        LOG_INFO("begin Interpreter, row_count = %d, col_count = %d", self._row_count, self._col_count)

        # 配置输出表名称
        self._output.append("#\t" + self._sheetName + "\n")
        
        # 先找到定义ID的列
        id_col = 0

        # 遍历表格解析数据结构
        for self._row in range(FIELD_VALUE_ROW - 1, self._row_count):
            # 如果 id 是 空 直接跳过改行
            info_id = str(self._sheet.cell_value(self._row, id_col)).strip()
            if info_id == "":
                LOG_WARN("%d is None", self._row)
                continue
            
            self._item = []
            
            for self._col in range(0, self._col_count):
                self._FieldDefine(self._row, self._col)

            print(info_id)
            print(self._item)

            self._items.append(self._item)
            self._item.clear()

        #  写文档，在这里要修改路径----------------------------------------------------------y要改
        #self._Write2File()
        print(self._items)
        LogHelp.close()

    def _FieldDefine(self, row, col):
        LOG_INFO("row=%d, col=%d", row, col)
        _field = {}
        # 获取“属性名行”也就是第1行的self._col列，得到这一列对应的属性名
        _field["field_name"] = str(self._sheet.cell_value(FIELD_NAME_ROW, col)).strip()
        # 获取“类型行”也就是第2行的self._col列,得到此列的类型
        _field["field_type"] = str(self._sheet.cell_value(FIELD_TYPE_ROW, col)).strip()
        # 获取“注释行”也就是第3行的self._col列，得到这一列对应的注释
        _field["field_comment"] = str(self._sheet.cell_value(FIELD_COMMENT_ROW, col))
        # 获取“数据行”也就是第4行的self._col列，得到这一列对应的注释
        _field["field_value"] = str(self._sheet.cell_value(FIELD_VALUE_ROW, col))
        #将字段的属性信息保存到字段列表
        self._item.append(_field)

    def _Write2File(self):
        """输出到文件"""
        pb_file = open(file=self._pb_file_name, mode="w+", encoding="utf-8")
        pb_file.writelines(self._output)
        pb_file.close()

class DataParser:
    """解析excel的数据"""

if __name__ == '__main__':
    """入口"""
    if len(sys.argv) < 2:
        print("Usage: %s xls_file_dir" % (sys.argv[0]))
        sys.exit(-1)

    xls_file_path = sys.argv[1]
    if len(xls_file_path) > 0:
        for root, dirs, files in os.walk(xls_file_path):
            for name in files:
                # delete the log and test result
                if name.startswith("~$"):
                    continue
                if (not name.endswith(".xls")) and (not name.endswith(".xlsx")) and (not name.endswith(".xlsm")):
                    continue

                xls_file_name = xls_file_path + "/" + name
                txtName = name.split(".")[0]

                tool = SheetInterpreter(xls_file_name,txtName)
                tool.OutputDataBySheet()