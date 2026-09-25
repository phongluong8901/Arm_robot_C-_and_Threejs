from pathlib import Path
import struct
import sys


def read_stl(path):
    data = path.read_bytes()
    is_binary = len(data) >= 84 and 84 + struct.unpack_from('<I', data, 80)[0] * 50 == len(data)
    triangles = []
    if is_binary:
        count = struct.unpack_from('<I', data, 80)[0]
        offset = 84
        for _ in range(count):
            values = struct.unpack_from('<12f', data, offset)
            triangles.append((values[3:6], values[6:9], values[9:12]))
            offset += 50
    else:
        vertices = []
        for line in data.decode('utf-8', errors='ignore').splitlines():
            parts = line.strip().split()
            if len(parts) == 4 and parts[0].lower() == 'vertex':
                vertices.append(tuple(float(value) for value in parts[1:]))
        triangles = [tuple(vertices[index:index + 3]) for index in range(0, len(vertices), 3)]
    return triangles


def write_obj(source, target):
    triangles = read_stl(source)
    with target.open('w', encoding='ascii') as output:
        output.write(f'# Converted from {source.name}\n')
        vertex_index = 1
        for triangle in triangles:
            for vertex in triangle:
                output.write(f'v {vertex[0]:.7g} {vertex[1]:.7g} {vertex[2]:.7g}\n')
            output.write(f'f {vertex_index} {vertex_index + 1} {vertex_index + 2}\n')
            vertex_index += 3


if __name__ == '__main__':
    folder = Path(sys.argv[1])
    for source in sorted(folder.glob('*.stl')):
        target = source.with_suffix('.obj')
        write_obj(source, target)
        print(f'{source.name} -> {target.name}')
