import pyodbc
import json
import os

# Configuration
server = r'(localdb)\mssqllocaldb'
database = 'FantasyDB'
output_dir_SeedData = r'F:\FantasyDatabase\FantasyDatabaseManager\FantasyDB\Seed Data'
output_dir_FKs = r'F:\FantasyDatabase\FantasyDatabaseManager\FantasyDB\Seed Data with FKs'

conn_str = f'DRIVER={{ODBC Driver 17 for SQL Server}};SERVER={server};DATABASE={database};Trusted_Connection=yes;'

def clean_for_json(value):
    if isinstance(value, bytes):
        return value.decode('utf-8', errors='ignore')
    return value

def get_foreign_keys(cursor, table_name):
    # Use sys tables to identify FK columns reliably
    cursor.execute("""
        SELECT COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ForeignKeyColumn
        FROM sys.foreign_key_columns AS fc
        JOIN sys.objects AS t ON fc.parent_object_id = t.object_id
        WHERE t.name = ?
    """, table_name)
    return {row.ForeignKeyColumn for row in cursor.fetchall()}

try:
    conn = pyodbc.connect(conn_str)
    cursor = conn.cursor()

    cursor.execute("""
        SELECT TABLE_NAME 
        FROM INFORMATION_SCHEMA.TABLES 
        WHERE TABLE_TYPE = 'BASE TABLE' 
        AND TABLE_CATALOG = ? 
        AND TABLE_NAME NOT LIKE 'sys%'
    """, database)

    tables = [row.TABLE_NAME for row in cursor.fetchall()]
    print(f"📦 Found {len(tables)} tables.")

    for table_name in tables:
        print(f"\n🔄 Exporting table: {table_name}")
        try:
            cursor.execute(f"SELECT * FROM [{table_name}]")
            all_columns = [col[0] for col in cursor.description]
            if not all_columns:
                print(f"⚠️ No columns found in {table_name}")
                continue

            id_column = all_columns[0]
            fk_columns = get_foreign_keys(cursor, table_name)
            non_fk_columns = [col for col in all_columns if col != id_column and col not in fk_columns]

            # --- Structure-only export (no Id, no FK)
            structure_rows = []
            if non_fk_columns:
                select_structure = ", ".join(f"[{col}]" for col in non_fk_columns)
                cursor.execute(f"SELECT {select_structure} FROM [{table_name}]")
                structure_rows = [
                    {col: clean_for_json(val) for col, val in zip(non_fk_columns, row)}
                    for row in cursor.fetchall()
                ]
            else:
                print(f"⚠️ No structure columns in {table_name}, exporting empty structure file.")

            os.makedirs(output_dir_SeedData, exist_ok=True)
            structure_path = os.path.join(output_dir_SeedData, f"{table_name}.json")
            with open(structure_path, 'w', encoding='utf-8') as f:
                json.dump(structure_rows, f, ensure_ascii=False, indent=4)
            print(f"✅ Exported structure-only data → {structure_path} ({len(structure_rows)} rows)")

            # --- FK-only export (Id + FKs)
            fk_rows = []
            if fk_columns:
                select_fk = ", ".join([f"[{id_column}]"] + [f"[{fk}]" for fk in fk_columns])
                cursor.execute(f"SELECT {select_fk} FROM [{table_name}]")
                fk_rows = [
                    {col: clean_for_json(val) for col, val in zip([id_column] + list(fk_columns), row)}
                    for row in cursor.fetchall()
                ]
            else:
                print(f"ℹ️ No FK columns found in {table_name}, exporting empty FK file.")

            os.makedirs(output_dir_FKs, exist_ok=True)
            fk_path = os.path.join(output_dir_FKs, f"{table_name}.json")
            with open(fk_path, 'w', encoding='utf-8') as f:
                json.dump(fk_rows, f, ensure_ascii=False, indent=4)
            print(f"✅ Exported FK-only data → {fk_path} ({len(fk_rows)} rows)")

        except Exception as e:
            print(f"❌ Error exporting {table_name}: {e}")

except Exception as global_error:
    print(f"\n💥 Global error: {global_error}")

finally:
    if 'conn' in locals() and conn:
        conn.close()
