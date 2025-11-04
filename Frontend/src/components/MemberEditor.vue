<template>
  <div v-if="visible" class="modal-overlay" @click="handleClose">
    <div class="modal-content" @click.stop>
      <div class="modal-header">
        <h2>{{ isEdit ? '编辑成员' : '添加成员' }}</h2>
        <button class="close-btn" @click="handleClose">×</button>
      </div>
      
      <div class="modal-body">
        <div class="form-group">
          <label>姓名 *</label>
          <input v-model="formData.name" type="text" placeholder="请输入姓名" />
        </div>

        <div class="form-group">
          <label>性别 *</label>
          <select v-model="formData.gender">
            <option value="Male">男</option>
            <option value="Female">女</option>
          </select>
        </div>

        <div class="form-group">
          <label>世代 *</label>
          <input v-model.number="formData.generation" type="number" min="1" placeholder="第几代" />
        </div>

        <div class="form-group">
          <label>出生日期</label>
          <input v-model="formData.birthDate" type="date" />
        </div>

        <div class="form-group">
          <label>去世日期</label>
          <input v-model="formData.deathDate" type="date" />
        </div>

        <div class="form-group">
          <label>父亲</label>
          <select v-model="formData.fatherId">
            <option :value="undefined">无</option>
            <option v-for="member in maleMembers" :key="member.id" :value="member.id">
              {{ member.name }} (第{{ member.generation }}代)
            </option>
          </select>
        </div>

        <div class="form-group">
          <label>母亲</label>
          <select v-model="formData.motherId">
            <option :value="undefined">无</option>
            <option v-for="member in femaleMembers" :key="member.id" :value="member.id">
              {{ member.name }} (第{{ member.generation }}代)
            </option>
          </select>
        </div>

        <div class="form-group">
          <label>配偶</label>
          <select v-model="formData.spouseId">
            <option :value="undefined">无</option>
            <option v-for="member in availableMembers" :key="member.id" :value="member.id">
              {{ member.name }} (第{{ member.generation }}代)
            </option>
          </select>
        </div>

        <div class="form-group">
          <label>头像URL</label>
          <input v-model="formData.avatarUrl" type="text" placeholder="请输入头像URL" />
        </div>

        <div class="form-group">
          <label>节点颜色</label>
          <input v-model="nodeColor" type="color" />
        </div>

        <div class="form-group">
          <label>备注</label>
          <textarea v-model="formData.notes" rows="3" placeholder="请输入备注信息"></textarea>
        </div>

        <div class="form-group">
          <label>自定义属性 (JSON)</label>
          <textarea v-model="formData.customAttributes" rows="3" placeholder='{"key": "value"}'></textarea>
        </div>
      </div>

      <div class="modal-footer">
        <button class="btn btn-cancel" @click="handleClose">取消</button>
        <button class="btn btn-primary" @click="handleSubmit">{{ isEdit ? '保存' : '创建' }}</button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import type { FamilyMember, CreateFamilyMemberRequest, UpdateFamilyMemberRequest } from '@/types'

/**
 * 组件属性
 */
interface Props {
  visible: boolean
  member?: FamilyMember
  members: FamilyMember[]
  familyId: string
}

const props = defineProps<Props>()

/**
 * 事件定义
 */
const emit = defineEmits<{
  close: []
  submit: [data: CreateFamilyMemberRequest | UpdateFamilyMemberRequest]
}>()

/**
 * 表单数据
 */
const formData = ref<any>({
  name: '',
  gender: 'Male',
  generation: 1,
  birthDate: undefined,
  deathDate: undefined,
  fatherId: undefined,
  motherId: undefined,
  spouseId: undefined,
  avatarUrl: undefined,
  notes: undefined,
  customAttributes: undefined
})

const nodeColor = ref('#5B8FF9')

/**
 * 是否为编辑模式
 */
const isEdit = computed(() => !!props.member)

/**
 * 男性成员列表
 */
const maleMembers = computed(() => 
  props.members.filter(m => m.gender === 'Male' && m.id !== props.member?.id)
)

/**
 * 女性成员列表
 */
const femaleMembers = computed(() => 
  props.members.filter(m => m.gender === 'Female' && m.id !== props.member?.id)
)

/**
 * 可选成员列表（排除自己）
 */
const availableMembers = computed(() => 
  props.members.filter(m => m.id !== props.member?.id)
)

/**
 * 初始化表单
 */
const initForm = () => {
  if (props.member) {
    formData.value = {
      name: props.member.name,
      gender: props.member.gender,
      generation: props.member.generation,
      birthDate: props.member.birthDate?.split('T')[0],
      deathDate: props.member.deathDate?.split('T')[0],
      fatherId: props.member.fatherId,
      motherId: props.member.motherId,
      spouseId: props.member.spouseId,
      avatarUrl: props.member.avatarUrl,
      notes: props.member.notes,
      customAttributes: props.member.customAttributes
    }

    // 解析节点样式
    if (props.member.nodeStyle) {
      try {
        const style = JSON.parse(props.member.nodeStyle)
        nodeColor.value = style.fill || '#5B8FF9'
      } catch (e) {
        console.error('解析节点样式失败', e)
      }
    }
  } else {
    formData.value = {
      name: '',
      gender: 'Male',
      generation: 1,
      birthDate: undefined,
      deathDate: undefined,
      fatherId: undefined,
      motherId: undefined,
      spouseId: undefined,
      avatarUrl: undefined,
      notes: undefined,
      customAttributes: undefined
    }
    nodeColor.value = '#5B8FF9'
  }
}

/**
 * 处理关闭
 */
const handleClose = () => {
  emit('close')
}

/**
 * 处理提交
 */
const handleSubmit = () => {
  if (!formData.value.name) {
    alert('请输入姓名')
    return
  }

  // 构建节点样式
  const nodeStyle = JSON.stringify({
    fill: nodeColor.value
  })

  const data: any = {
    ...formData.value,
    nodeStyle
  }

  if (!isEdit.value) {
    data.familyId = props.familyId
  }

  emit('submit', data)
}

// 监听visible变化，重新初始化表单
watch(() => props.visible, (newVal) => {
  if (newVal) {
    initForm()
  }
})
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  border-radius: 8px;
  width: 90%;
  max-width: 600px;
  max-height: 90vh;
  display: flex;
  flex-direction: column;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
}

.modal-header {
  padding: 20px;
  border-bottom: 1px solid #e8e8e8;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.modal-header h2 {
  margin: 0;
  font-size: 20px;
  color: #333;
}

.close-btn {
  background: none;
  border: none;
  font-size: 28px;
  color: #999;
  cursor: pointer;
  padding: 0;
  width: 30px;
  height: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.close-btn:hover {
  color: #333;
}

.modal-body {
  padding: 20px;
  overflow-y: auto;
  flex: 1;
}

.form-group {
  margin-bottom: 16px;
}

.form-group label {
  display: block;
  margin-bottom: 8px;
  font-weight: 500;
  color: #333;
}

.form-group input,
.form-group select,
.form-group textarea {
  width: 100%;
  padding: 8px 12px;
  border: 1px solid #d9d9d9;
  border-radius: 4px;
  font-size: 14px;
  transition: border-color 0.3s;
}

.form-group input:focus,
.form-group select:focus,
.form-group textarea:focus {
  outline: none;
  border-color: #40a9ff;
}

.form-group input[type="color"] {
  height: 40px;
  cursor: pointer;
}

.modal-footer {
  padding: 16px 20px;
  border-top: 1px solid #e8e8e8;
  display: flex;
  justify-content: flex-end;
  gap: 12px;
}

.btn {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.3s;
}

.btn-cancel {
  background: #fff;
  border: 1px solid #d9d9d9;
  color: #333;
}

.btn-cancel:hover {
  border-color: #40a9ff;
  color: #40a9ff;
}

.btn-primary {
  background: #1890ff;
  color: white;
}

.btn-primary:hover {
  background: #40a9ff;
}
</style>
